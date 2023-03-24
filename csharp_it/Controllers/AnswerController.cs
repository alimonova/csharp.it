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
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _service;
        private readonly IAccountService _account;
        private readonly IQuestionService _questions;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerService service, IMapper mapper,
            IAccountService account, IQuestionService questions)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _questions = questions;
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var answer = await _service.GetAnswerByIdAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            var courseId = answer.Question.Lesson.Chapter.CourseId;

            if (await _account.CheckAccessToCourse(courseId, "SEE_RIGHT_ANSWERS_EXPLANATIONS")
                || await _account.CheckAccessToCourse(courseId, "SEE_ANSWERS_AND_CHECK_TEST"))
            {
                return Ok(_mapper.Map<AnswerDto>(answer));
            }
            else
            {   
                return Forbid();
            }
        }

        [HttpGet("ReadByQuestionId/{questionId}")]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetByQuestion(int questionId)
        {
            var user = await _account.GetCurrentUserAsync();
            var question = await _questions.GetQuestionByIdAsync(questionId);

            if (question == null)
            {
                return BadRequest();
            }

            var course = question.Lesson.Chapter.Course;
            
            if (user.Id != course.Teacher.UserId)
            {
                var answers = await _service.GetAnswersByQuestionIdAsync(questionId);
                return Ok(_mapper.Map<IEnumerable<AnswerRightDto>>(answers));
            }
            else if (await _account.CheckAccessToCourse(course.Id, "SEE_RIGHT_ANSWERS_EXPLANATIONS")
                || await _account.CheckAccessToCourse(course.Id, "SEE_ANSWERS_AND_CHECK_TEST"))
            {
                var answers = await _service.GetAnswersByQuestionIdAsync(questionId);
                return Ok(_mapper.Map<IEnumerable<AnswerDto>>(answers));
            }

            return Forbid();
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAnswer(AnswerRightDto answer)
        {
            var user = await _account.GetCurrentUserAsync();
            var question = await _questions.GetQuestionByIdAsync(answer.QuestionId);
            var course = question.Lesson.Chapter.Course;
            if (user.Id != course.Teacher.UserId)
            {
                return Forbid();
            }

            var _answer = _mapper.Map<Answer>(answer);
            return Created("Answer was created successfully",
                _mapper.Map<AnswerDto>(await _service.CreateAnswerAsync(_answer)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateAnswer(AnswerDto answer)
        {
            var user = await _account.GetCurrentUserAsync();
            var question = await _questions.GetQuestionByIdAsync(answer.QuestionId);
            if (user.Id != question.Lesson.Chapter.Course.Teacher.UserId)
            {
                return Forbid();
            }

            var _answer = _mapper.Map<Answer>(answer);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<AnswerDto>(await _service.UpdateAnswerAsync(_answer)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var answer = await _service.GetAnswerByIdAsync(id);

            if (answer == null)
            {
                return BadRequest();
            }

            if (user.Id != answer.Question.Lesson.Chapter.Course.Teacher.UserId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(answer);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

