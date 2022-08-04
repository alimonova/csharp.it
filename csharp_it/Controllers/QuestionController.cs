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
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;
        private readonly IAccountService _account;
        private readonly ILessonService _lessons;
        private readonly IMapper _mapper;

        public QuestionController(IQuestionService service, IMapper mapper,
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
            var question = await _service.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            var courseId = question.Lesson.Chapter.CourseId;

            if (await _account.CheckAccessToCourse(courseId, "SEE_RIGHT_ANSWERS_EXPLANATIONS"))
            {
                return Ok(_mapper.Map<QuestionDto>(question));
            }
            else if (await _account.CheckAccessToCourse(courseId, "SEE_QUESTIONS"))
            {
                return Ok(_mapper.Map<QuestionNoExplanationDto>(question));
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet("ReadByLessonId/{lessonId}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetByLesson(int lessonId)
        {
            var lesson = await _lessons.GetLessonByIdAsync(lessonId);

            if (lesson == null)
            {
                return BadRequest();
            }

            var course = lesson.Chapter.Course;

            if (await _account.CheckAccessToCourse(course.Id, "SEE_RIGHT_ANSWERS_EXPLANATIONS"))
            {
                var questions = await _service.GetQuestionsByLessonIdAsync(lessonId);
                return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questions));
            }
            else if (await _account.CheckAccessToCourse(course.Id, "SEE_QUESTIONS"))
            {
                var questions = await _service.GetQuestionsByLessonIdAsync(lessonId);
                return Ok(_mapper.Map<IEnumerable<QuestionNoExplanationDto>>(questions));
            }

            return Forbid();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateQuestion(QuestionDto question)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(question.LessonId);
            var course = lesson.Chapter.Course;
            if (user.Id != course.AuthorId)
            {
                return Forbid();
            }

            var _question = _mapper.Map<Question>(question);
            return Created("Question was created successfully",
                _mapper.Map<QuestionDto>(await _service.CreateQuestionAsync(_question)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateQuestion(QuestionDto question)
        {
            var user = await _account.GetCurrentUserAsync();
            var lesson = await _lessons.GetLessonByIdAsync(question.LessonId);
            if (user.Id != lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            var _question = _mapper.Map<Question>(question);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<QuestionDto>(await _service.UpdateQuestionAsync(_question)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var question = await _service.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return BadRequest();
            }

            if (user.Id != question.Lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(question);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

