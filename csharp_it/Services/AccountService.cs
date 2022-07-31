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

        public async Task<User> UpdateAccountWallet(double sum)
        {
            currentUser.WalletMoney += sum;

            if (currentUser.WalletMoney < 0)
            {
                return null;
            }

            _dbcontext.Users.Attach(currentUser);
            _dbcontext.Entry(currentUser).Property(x => x.WalletMoney).IsModified = true;
            await _dbcontext.SaveChangesAsync();

            return currentUser;
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

        public async Task<UserCourse> BuyCourse(Guid tarifId)
        {
            var tarif = await _dbcontext.Tarifs.FirstOrDefaultAsync(x => x.Id == tarifId);

            if (tarif == null)
            {
                return null;
            }

            var userCourse = new UserCourse { CourseId = tarif.CourseId, TarifId = tarif.Id };

            await _dbcontext.UserCourses.AddAsync(userCourse);
            var result = await UpdateAccountWallet(tarif.Price);

            if (result == null)
            {
                return null;
            }

            return userCourse;
        }
    }
}

