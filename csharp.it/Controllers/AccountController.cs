using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using csharp.it.Dto;
using csharp.it.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace csharp.it.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        readonly ILogger<AccountController> _log;
        private readonly TokenSettings _appSettings;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<User> userManager, IMapper mapper,
            DbContext context, ILogger<AccountController> log,
            IOptions<TokenSettings> appSettings,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _log = log;
            _appSettings = appSettings.Value;
            _configuration = configuration;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register(string Email, string FirstName,
            string LastName, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Email = Email, UserName = Email,
                    FirstName = FirstName, LastName = LastName };
                var result = await _userManager.CreateAsync(user, Password);
                _context.SaveChanges();
                
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                await _userManager.AddToRoleAsync(user, "student");
                return Created("", "Пользователь успешно создан.");
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
            var _teachers = _mapper.Map<IEnumerable<UserDto>>(teachers);
            return Ok(teachers);
        }

        [Authorize]
        [HttpGet("Students")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var _students = _mapper.Map<IEnumerable<UserDto>>(students);
            return Ok(_students);
        }

        [HttpPost("Authorization")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthorization model)
        {

            var users = _context.Users.Where(x => x.Email == model.Email).ToList();
            User user = null;

            if (users.Count() > 0)
            {
                user = users[0];
            }

            if (user == null ||
                !await this._userManager.CheckPasswordAsync(user, model.Password))
            {
                _log.LogWarning($"AUTHORIZATION: User was not logged in " +
                    $"(wrong password). Returned unauthorized.");
                return Unauthorized();
            }

            var roleId = _context.UserRoles.First(x => x.UserId == user.Id).RoleId;
            var role = _context.Roles.First(x => x.Id == roleId).Name;

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
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                Token = encodedJwt,
                Id = user.Id,
                UserName = user.UserName,
                ValidTo = jwt.ValidTo
            });
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            if (claims.Count() != 0)
            {
                var id = Guid.Parse(claims.First(x => x.Type == "UserID").Value);

                IList<string> roles = new List<string> { "Роль не определена" };
                User user = _context.Users.First(x => x.Id == id);
                if (user != null)
                    roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }

            return Unauthorized();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Roles/{email}")]
        public async Task<IActionResult> GetRolesByEmail(string email)
        {
            IList<string> roles = new List<string> { "Роль не определена" };
            User user = _context.Users.First(x => x.Email == email);
            if (user != null)
                roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
    }
}

