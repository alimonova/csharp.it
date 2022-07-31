using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using csharp_it.Dto;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csharp_it.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly IAccountService _account;
        private readonly IMapper _mapper;

        public CourseController(ICourseService service, IMapper mapper,
            IAccountService account)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
        }

        [HttpGet("Read")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> Get()
        {
            var courses = await _service.GetCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _service.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpGet("ReadByStudentId/{userId}")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetByStudent(Guid userId)
        {
            var roles = await _account.GetCurrentUserRoles();
            var user = await _account.GetCurrentUserAsync();


            if (!roles.ToList().Contains("ADMIN") && user.Id != userId)
            {
                return Forbid();
            }

            var channels = await _service.GetCoursesByStudentIdAsync(userId);
            return Ok(channels);
        }

        [HttpGet("ReadByTeacherId/{teacherId}")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetByTeacher(Guid userId)
        {
            var channels = await _service.GetCoursesByTeacherIdAsync(userId);
            return Ok(channels);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCourse(CourseDto course)
        {
            var roles = await _account.GetCurrentUserRoles();

            if (!roles.ToList().Contains("TEACHER"))
            {
                return Forbid();
            }
            var user = await _account.GetCurrentUserAsync();
            course.AuthorId = user.Id;
            var _course = _mapper.Map<Course>(course);
            return Created("Course was created successfully", await _service.CreateCourseAsync(_course));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateCourse(CourseDto course)
        {
            var user = await _account.GetCurrentUserAsync();
            if (user.Id == course.AuthorId)
            {
                var _course = _mapper.Map<Course>(course);
                return StatusCode((int)HttpStatusCode.NoContent, await _service.UpdateCourseAsync(_course));
            }

            return Forbid();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var course = await _service.GetCourseByIdAsync(id);

            if (course == null)
            {
                BadRequest();
            }

            if (user.Id == course.AuthorId)
            {
                await _service.DeleteAsync(id);
                return StatusCode((int)HttpStatusCode.NoContent);
            }

            return Forbid();
        }
    }
}

