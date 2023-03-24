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
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _service;
        private readonly IAccountService _account;
        private readonly IChapterService _chapters;
        private readonly IMapper _mapper;

        public LessonController(ILessonService service, IMapper mapper,
            IAccountService account, IChapterService chapters)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _chapters = chapters;
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lesson = await _service.GetLessonByIdAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            if (await _account.CheckAccessToCourse(lesson.Chapter.Course.Id, "SEE_LESSONS"))
            {
                return Ok(_mapper.Map<LessonDto>(lesson));
            }

            return Forbid();
        }

        [HttpGet("ReadByChapterId/{chapterId}")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetByChapter(int chapterId)
        {
            var chapter = await _chapters.GetChapterByIdAsync(chapterId);

            if (chapter == null)
            {
                return BadRequest();
            }

            var course = chapter.Course;
            
            if (await _account.CheckAccessToCourse(course.Id, "SEE_LESSONS"))
            {
                var lessons = await _service.GetLessonsByChapterIdAsync(chapterId);
                return Ok(_mapper.Map<IEnumerable<LessonDto>>(lessons));
            }

            return Forbid();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateLesson(LessonDto lesson)
        {
            var user = await _account.GetCurrentUserAsync();
            var chapter = await _chapters.GetChapterByIdAsync(lesson.ChapterId);
            if (user.Id != chapter.Course.Teacher.UserId)
            {
                return Forbid();
            }

            var _lesson = _mapper.Map<Lesson>(lesson);
            return Created("Lesson was created successfully", _mapper.Map<LessonDto>(await _service.CreateLessonAsync(_lesson)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateLesson(LessonDto lesson)
        {
            var user = await _account.GetCurrentUserAsync();
            var chapter = await _chapters.GetChapterByIdAsync(lesson.ChapterId);
            if (user.Id != chapter.Course.Teacher.UserId)
            {
                return Forbid();
            }

            var _lesson = _mapper.Map<Lesson>(lesson);
            return StatusCode((int)HttpStatusCode.NoContent, _mapper.Map<LessonDto>(await _service.UpdateLessonAsync(_lesson)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _service.GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return BadRequest();
            }

            if (user.Id != lesson.Chapter.Course.Teacher.UserId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(lesson);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPost("CheckTest")]
        public async Task<IActionResult> CheckTest([FromBody] CheckTest model)
        {
            var lesson = await _service.GetLessonByIdAsync(model.lessonId);
            if (!await _account.CheckAccessToCourse(lesson.Chapter.Course.Id,
                "SEE_ANSWERS_AND_CHECK_TEST"))
            {
                return BadRequest();
            }

            var result = await this._service.CheckTestAsync(model.answers, lesson);
            return Ok(result);
        }

        [HttpGet("GetRightAnswers")]
        public async Task<IActionResult> GetRightAnswers(int id)
        {
            var lesson = await _service.GetLessonByIdAsync(id);
            if (!await _account.CheckAccessToCourse(lesson.Chapter.CourseId,
                "SEE_ANSWERS_AND_CHECK_TEST"))
            {
                return BadRequest();
            }

            var result = _service.GetRightAnswersByLesson(lesson);
            return Ok(result);
        }
    }
}

