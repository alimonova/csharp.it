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
        private readonly ICourseService _courses;
        private readonly ILessonService _lessons;
        private readonly IMapper _mapper;

        public QuestionController(IQuestionService service, IMapper mapper,
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

        [HttpGet("ReadByLessonId/{courseId}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetByLesson(int lessonId)
        {
            var questions = await _service.GetQuestionsByLessonIdAsync(lessonId);

            if (questions == null)
            {
                return NotFound();
            }

            var courseId = questions.First().Lesson.Chapter.CourseId;

            if (await _account.CheckAccessToCourse(courseId, "SEE_RIGHT_ANSWERS_EXPLANATIONS"))
            {
                return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questions));
            }
            else if (await _account.CheckAccessToCourse(courseId, "SEE_QUESTIONS"))
            {
                return Ok(_mapper.Map<IEnumerable<QuestionNoExplanationDto>>(questions));
            }
            else
            {
                return Forbid();
            }
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
            return Created("Question was created successfully", await _service.CreateQuestionAsync(_question));
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
            return StatusCode((int)HttpStatusCode.NoContent, await _service.UpdateQuestionAsync(_question));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var question = await _service.GetQuestionByIdAsync(id);

            if (question == null)
            {
                BadRequest();
            }

            if (user.Id != question.Lesson.Chapter.Course.AuthorId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

