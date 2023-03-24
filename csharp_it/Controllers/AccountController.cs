using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using csharp_it.Dto;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace csharp_it.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _service;
        private readonly ICourseService _courses;
        private readonly IEmailSender _sender;

        public AccountController(IMapper mapper, IAccountService service,
            ICourseService courses, IEmailSender sender)
        {
            _mapper = mapper;
            _service = service;
            _courses = courses;
            _sender = sender;
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<UserDto> GetCurrentUser()
        {
            return _mapper.Map<UserDto>(await _service.GetCurrentUserAsync());
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Register(model);
                var token = await _service.SetEmailConfirmationCode(model.Email);
                var html = _service.FormLetterEmailConfirmation(token, model.Email);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                try
                {
                    await _sender.SendEmailAsync(model.Email, "Email Confirmation", html);
                }
                catch
                {
                    return BadRequest("Email was not sent. Please contact administrator.");
                }

                return Created("", result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Teachers")]
        public async Task<ActionResult<UserDto>> GetTeachers()
        {
            var teachers = await _service.GetUsersOfRole("TEACHER");
            var _teachers = _mapper.Map<IEnumerable<UserDto>>(teachers);
            return Ok(_teachers);
        }

        [Authorize(Roles = "ADMIN, TEACHER")]
        [HttpGet("Students")]
        public async Task<ActionResult<UserDto>> GetStudents()
        {
            var students = await _service.GetUsersOfRole("STUDENT");
            var _students = _mapper.Map<IEnumerable<UserDto>>(students);
            return Ok(_students);
        }

        [HttpPost("Authentication")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthorization model)
        {
            var jwt = await _service.Authenticate(model);

            if (jwt == null)
            {
                return Unauthorized();
            }

            /*if (!await _service.CheckEmailConfirmationAsync(model.Email))
            {
                return Forbid();
            }*/

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var user = await _service.GetUserByEmailAsync(model.Email);

            return Ok(new
            {
                Token = encodedJwt,
                ValidTo = jwt.ValidTo,
                User = _mapper.Map<UserDto>(user)
            });
        }

        [Authorize]
        [HttpGet("MyRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _service.GetCurrentUserRoles();
            return Ok(roles);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Roles/{email}")]
        public async Task<IActionResult> GetRolesByEmail(string email)
        {
            var roles = await _service.GetRolesByUserEmail(email);

            if (roles == null)
            {
                return BadRequest();
            }

            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetTransactions")]
        public ActionResult<Transaction[]> Get(string name)
        {
            var transactions = _service.GetTransactions();

            if (transactions == null)
            {
                return NoContent();
            }

            return Ok(transactions);
        }

        [HttpPost("BuyCourse")]
        public async Task<IActionResult> BuyCourse([FromBody]BuyCourseModel model)
        {
            var result = await _service.BuyCourse(model.TarifId, model.Month);

            if (result == -1)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("ReadStudentsByCourseId/{courseId}")]
        public async Task<IActionResult> GetStudentsByCourse(int courseId)
        {
            var course = await _courses.GetCourseByIdAsync(courseId);
            var user = await _service.GetCurrentUserAsync();
            if (course.Teacher.UserId != user.Id)
            {
                return Forbid();
            }

            var students = _service.GetStudentsOfCourse(courseId);
            var _students = _mapper.Map<IEnumerable<UserDto>>(students);
            return Ok(_students);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var result = await _service.ConfirmEmail(token, email);

            if (result != null && result.Succeeded)
            {
                return Ok("Email was successfully confirmed");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var link = await _service.ForgotPassword(model.Email);
            var html = _service.FormLetterPasswordRecovery(link);
               
            try
            {
                await _sender.SendEmailAsync(model.Email, "Password Recovery", html);
            }
            catch
            {
                return BadRequest("Email was not sent. Please contact administrator.");
            }

            return Ok();
            
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.ResetPassword(model);

                if (result != null && result.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}

