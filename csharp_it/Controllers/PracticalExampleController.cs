using System;
using System.Net;
using AutoMapper;
using csharp_it.Dto;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csharp_it.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PracticalExampleController : ControllerBase
    {
        private readonly IPracticalExampleService _service;
        private readonly IAccountService _account;
        private readonly ICourseService _courses;
        private readonly ILessonService _lessons;
        private readonly IMapper _mapper;

        public PracticalExampleController(IPracticalExampleService service, IMapper mapper,
            IAccountService account, ICourseService courses,
            ILessonService lessons)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _courses = courses;
            _lessons = lessons;
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var example = await _service.GetPracticalExampleByIdAsync(id);

            if (example == null)
            {
                return NotFound();
            }

            if (await _account.CheckAccessToCourse(
                example.Lesson.Chapter.CourseId, "SEE_PRACTICAL_EXAMPLES"))
            {
                return Ok(_mapper.Map<PracticalExampleDto>(example));
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet("ReadByLessonId/{lessonId}")]
        public async Task<ActionResult> GetByLesson(int lessonId)
        {
            var examples = await _service.GetPracticalExamplesByLessonIdAsync(lessonId);

            if (examples == null)
            {
                return NotFound();
            }

            if (await _account.CheckAccessToCourse(
                examples.First().Lesson.Chapter.CourseId, "SEE_PRACTICAL_EXAMPLES"))
            {
                return Ok(_mapper.Map<IEnumerable<PracticalExampleDto>>(examples));
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePracticalExample(PracticalExampleDto example)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(example.LessonId);

            if (lesson == null)
            {
                return BadRequest();
            }

            var course = lesson.Chapter.Course;
            if (user.Id != course.AuthorId)
            {
                return Forbid();
            }

            var _example = _mapper.Map<PracticalExample>(example);
            return Created("PracticalExample was created successfully",
                _mapper.Map<PracticalExampleDto>(await _service.CreatePracticalExampleAsync(_example)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdatePracticalExample(PracticalExampleDto example)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(example.LessonId);
            if (user.Id != lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            var _example = _mapper.Map<PracticalExample>(example);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<PracticalExampleDto>(await _service.UpdatePracticalExampleAsync(_example)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var example = await _service.GetPracticalExampleByIdAsync(id);

            if (example == null)
            {
                return BadRequest();
            }

            if (user.Id != example.Lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

