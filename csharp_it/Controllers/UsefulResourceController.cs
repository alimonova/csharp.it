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
    public class UsefulResourceController : ControllerBase
    {
        private readonly IUsefulResourceService _service;
        private readonly IAccountService _account;
        private readonly ILessonService _lessons;
        private readonly IMapper _mapper;

        public UsefulResourceController(IUsefulResourceService service, IMapper mapper,
            IAccountService account, ILessonService lessons)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _lessons = lessons;
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var resource = await _service.GetUsefulResourceByIdAsync(id);

            if (resource == null)
            {
                return NotFound();
            }

            if (await _account.CheckAccessToCourse(
                resource.Lesson.Chapter.CourseId, "SEE_USEFUL_RESOURCES"))
            {
                return Ok(_mapper.Map<UsefulResourceDto>(resource));
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet("ReadByLessonId/{lessonId}")]
        public async Task<ActionResult> GetByLesson(int lessonId)
        {
            var resources = await _service.GetUsefulResourcesByLessonIdAsync(lessonId);

            if (resources.FirstOrDefault() == null)
            {
                return NotFound();
            }

            if (await _account.CheckAccessToCourse(
                resources.First().Lesson.Chapter.CourseId, "SEE_USEFUL_RESOURCES"))
            {
                return Ok(_mapper.Map<IEnumerable<UsefulResourceDto>>(resources));
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUsefulResource(UsefulResourceDto resource)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(resource.LessonId);

            if (lesson == null)
            {
                return BadRequest();
            }

            var course = lesson.Chapter.Course;
            if (user.Id != course.AuthorId)
            {
                return Forbid();
            }

            var _resource = _mapper.Map<UsefulResource>(resource);
            return Created("UsefulResource was created successfully",
                _mapper.Map<UsefulResourceDto>(await _service.CreateUsefulResourceAsync(_resource)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateUsefulResource(UsefulResourceDto resource)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(resource.LessonId);
            if (user.Id != lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            var _resource = _mapper.Map<UsefulResource>(resource);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<UsefulResourceDto>(await _service.UpdateUsefulResourceAsync(_resource)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var resource = await _service.GetUsefulResourceByIdAsync(id);

            if (resource == null)
            {
                return BadRequest();
            }

            if (user.Id != resource.Lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(resource);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

