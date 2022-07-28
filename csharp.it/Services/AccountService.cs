using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using csharp.it.Models;
using csharp.it.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace csharp.it.Services
{
	public class AccountService : IAccountService
	{
        private readonly UserManager<User> _userManager;
        private readonly Models.DbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenSettings _appSettings;
        private User currentUser;

        public AccountService(UserManager<User> userManager,
            Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IOptions<TokenSettings> appSettings)
        {
            _userManager = userManager;
            _dbcontext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
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
    }
}

