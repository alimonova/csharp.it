using System;
using System.Net;
using AutoMapper;
using csharp_it.Dto;
using csharp_it.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = csharp_it.Models.Task;

namespace csharp_it.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;
        private readonly IAccountService _account;
        private readonly ILessonService _lessons;
        private readonly IMapper _mapper;

        public TaskController(ITaskService service, IMapper mapper,
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
            var task = await _service.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var courseId = task.Lesson.Chapter.CourseId;

            if (await _account.CheckAccessToCourse(courseId, "SEE_TASKS"))
            {
                return Ok(_mapper.Map<TaskDto>(task));
            }
            
            return Forbid();
        }

        [HttpGet("ReadByLessonId/{lessonId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetByLesson(int lessonId)
        {
            var lesson = await _lessons.GetLessonByIdAsync(lessonId);

            if (lesson == null)
            {
                return BadRequest();
            }

            var course = lesson.Chapter.Course;

            if (await _account.CheckAccessToCourse(course.Id, "SEE_TASKS"))
            {
                var tasks = await _service.GetTasksByLessonIdAsync(lessonId);
                return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
            }

            return Forbid();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTask(TaskDto task)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(task.LessonId);
            var course = lesson.Chapter.Course;
            if (user.Id != course.AuthorId)
            {
                return Forbid();
            }

            var _task = _mapper.Map<Task>(task);
            return Created("Task was created successfully",
                _mapper.Map<TaskDto>(await _service.CreateTaskAsync(_task)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateTask(TaskDto task)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(task.LessonId);
            if (user.Id != lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            var _task = _mapper.Map<Task>(task);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<TaskDto>(await _service.UpdateTaskAsync(_task)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var task = await _service.GetTaskByIdAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            if (user.Id != task.Lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(task);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

