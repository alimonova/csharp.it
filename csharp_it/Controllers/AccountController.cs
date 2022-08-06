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

        public AccountController(IMapper mapper, IAccountService service,
            ICourseService courses)
        {
            _mapper = mapper;
            _service = service;
            _courses = courses;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Register(model);
                
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
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

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                Token = encodedJwt,
                ValidTo = jwt.ValidTo
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

        [HttpPost("BuyCourse/{tarifId}")]
        public async Task<IActionResult> BuyCourse(Guid tarifId)
        {
            var result = await _service.BuyCourse(tarifId);

            if (result == null)
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
            if (course.AuthorId != user.Id)
            {
                return Forbid();
            }

            var students = _service.GetStudentsOfCourse(courseId);
            var _students = _mapper.Map<IEnumerable<UserDto>>(students);
            return Ok(_students);
        }
    }
}

