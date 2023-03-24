using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Task = csharp_it.Models.Task;
using System.Web;
using Microsoft.AspNetCore.Mvc.Routing;

namespace csharp_it.Services
{
	public class AccountService : IAccountService
	{
        private readonly UserManager<User> _userManager;
        private readonly Models.DbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenSettings _appSettings;
        private readonly WayForPaySettings _wfpSettings;
        private User currentUser;

        public AccountService(UserManager<User> userManager,
            Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IOptions<TokenSettings> appSettings,
            IOptions<WayForPaySettings> wfpSettings)
        {
            _userManager = userManager;
            _dbcontext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _wfpSettings = wfpSettings.Value;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            if (currentUser == null)
            {
                var Id = Guid.Parse(_httpContextAccessor.HttpContext.User.
                   Claims.First(x => x.Type == "UserID").Value);
                currentUser = await _dbcontext.Users.FirstOrDefaultAsync(x =>
                x.Id == Id);
            }

            return currentUser;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> Register(UserRegistration model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            await _userManager.AddToRoleAsync(user, "STUDENT");
            await _dbcontext.SaveChangesAsync();

            return result;
        }

        public async Task<bool> CheckEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            bool confirmed = await _userManager.IsEmailConfirmedAsync(user);
            return confirmed;
        }

        public async Task<JwtSecurityToken> Authenticate(UserAuthorization model)
        {
            var users = _dbcontext.Users.ToList();

            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null ||
                !await this._userManager.CheckPasswordAsync(user, model.Password))
            {
                return null;
            }

            var roleId = _dbcontext.UserRoles.First(x => x.UserId == user.Id).RoleId;
            var role = _dbcontext.Roles.First(x => x.Id == roleId).Name;

            var claims = new List<Claim>();
            claims.Add(new Claim("UserID", user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, role));

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _appSettings.Issuer,
                    audience: _appSettings.Audience,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(_appSettings.Lifetime)),
                    signingCredentials: new SigningCredentials(_appSettings.
                    GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return jwt;
        }

        public async Task<IEnumerable<User>> GetUsersOfRole(string role)
        {
            return await _userManager.GetUsersInRoleAsync("Teacher");
        }

        public async Task<IEnumerable<string>> GetCurrentUserRoles()
        {
            var user = await GetCurrentUserAsync();
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<string>> GetRolesByUserEmail(string email)
        {
            var user = _dbcontext.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<double> UpdateAccountWallet(double sum)
        {
            var user = await GetCurrentUserAsync();
            user.WalletMoney += sum;

            if (currentUser.WalletMoney < 0)
            {
                return -1;
            }

            await _dbcontext.SaveChangesAsync();

            return user.WalletMoney;
        }

        public Transaction[] GetTransactions()
        {
            var merchant_account = _wfpSettings.Merchant;
            var date_end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var date_begin = new DateTimeOffset(DateTime.UtcNow.AddDays(-31)).ToUnixTimeSeconds();

            var strr = merchant_account + ";" + date_begin + ";" + date_end;

            var data = Encoding.UTF8.GetBytes(strr);
            var key = Encoding.UTF8.GetBytes(_wfpSettings.Key);
            var hmac = new HMACMD5(key);
            var hashBytes = hmac.ComputeHash(data);
            var hash = System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_wfpSettings.Api);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"transactionType\": \"TRANSACTION_LIST\"," +
                                    "\"merchantAccount\": \"" + merchant_account + "\"," +
                                     "\"merchantSignature\":\"" + hash + "\"," +
                                     "\"apiVersion\": \"1\"," +
                                     "\"dateBegin\": " + date_begin + "," +
                                     "\"dateEnd\": " + date_end + " }";

                streamWriter.Write(json);
            }

            var res_text = "";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                res_text = streamReader.ReadToEnd();
            }

            TransactionResponse result = null;
            try
            {
                result = JsonConvert.DeserializeObject<TransactionResponse>(res_text);
            }
            catch { }

            if (result.reasonCode == 1100)
            {
                return result.transactionList;
            }

            return null;
        }

        public async Task<double> BuyCourse(Guid tarifId, bool month = true)
        {
            var tarif = await _dbcontext.Tarifs.FirstOrDefaultAsync(x => x.Id == tarifId);

            if (tarif == null)
            {
                return -1;
            }

            var exp = month ? DateTime.Now.AddMonths(1) : DateTime.Now.AddYears(1);

            var user = await GetCurrentUserAsync();
            var userCourse = new UserCourse { TarifId = tarif.Id,
                UserId = user.Id, Expiration = DateTime.Now,
                Progress = 0 };

            await _dbcontext.UserCourses.AddAsync(userCourse);

            await _dbcontext.SaveChangesAsync();
            var price = month ? tarif.PriceMonth : tarif.PriceYear * 12;
            var result = await UpdateAccountWallet(price * (-1));

            return result;
        }

        public async Task<IEnumerable<User>> GetStudentsOfCourse(int courseId)
        {
            var course = await _dbcontext.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
            {
                return null;
            }

            var tarifsIds = course.Tarifs.Select(x=>x.Id);
            var userCourses = _dbcontext.UserCourses.Where(x => tarifsIds.Contains(x.TarifId));
            var users = new List<User>();

            foreach (var userCourse in userCourses)
            {
                users.Add(await _dbcontext.Users.FirstOrDefaultAsync(x=>x.Id == userCourse.UserId));
            }

            return users;
        }

        public async Task<bool> CheckAccessToCourse(int courseId, string accessName)
        {
            var course = await _dbcontext.Courses.FirstOrDefaultAsync(x=>x.Id == courseId);

            if (course == null)
            {
                return false;
            }

            var user = await GetCurrentUserAsync();
            if (course.Teacher.UserId == user.Id)
            {
                return true;
            }

            var access = _dbcontext.Accesses.FirstOrDefault(x => x.Name == accessName);

            if (access == null)
            {
                return false;
            }

            var userCourse = await _dbcontext.UserCourses.FirstOrDefaultAsync(
                x=> course.Tarifs.Select(t => t.Id).Contains(x.TarifId) && x.UserId == user.Id);

            if (userCourse == null)
            {
                return false;
            }

            var tarifAccess = await _dbcontext.TarifAccesses.FirstOrDefaultAsync(
                x => x.AccessId == access.Id && x.TarifId == userCourse.Tarif.Id);

            if (tarifAccess == null)
            {
                return false;
            }

            return true;
        }

        public async Task<string> SetEmailConfirmationCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return token;
        }

        public string FormLetterEmailConfirmation(string token, string email)
        {
            StringBuilder html = new StringBuilder();
            token = HttpUtility.UrlEncode(token);
            email = HttpUtility.UrlEncode(email);
            html.Append("To confirm your email on csharp_it please follow the link ");
            html.Append(_appSettings.Url + "confirm-email?token=" + token);
            html.Append("&email=" + email);
            html.Append(". If you didn't register on this site, please ignore this message.");

            return html.ToString();
        }

        public async Task<IdentityResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result;
        }

        public async Task<string> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return "";
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = HttpUtility.UrlEncode(code);
            var callbackUrl = "reset-password?userId=" + user.Id + "&code=" + code;

            return callbackUrl;
        }

        public async Task<IdentityResult> ResetPassword(ResetPassword model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return null;
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            return result;
        }

        public string FormLetterPasswordRecovery(string link)
        {
            StringBuilder html = new StringBuilder();
 
            html.Append("To reset your password on csharp_it please follow the link ");
            html.Append(_appSettings.Url + link);
            html.Append(". If you didn't try to reset password on this site, please ignore this message.");

            return html.ToString();
        }
    }
}

