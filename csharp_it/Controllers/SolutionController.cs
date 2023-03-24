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
    public class SolutionController : ControllerBase
    {
        private readonly ISolutionService _service;
        private readonly IAccountService _account;
        private readonly ITaskService _tasks;
        private readonly IMapper _mapper;

        public SolutionController(ISolutionService service, IMapper mapper,
            IAccountService account, ITaskService tasks)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _tasks = tasks;
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _account.GetCurrentUserAsync();
            var solution = await _service.GetSolutionByIdAsync(id);

            if (solution == null)
            {
                return NotFound();
            }

            var course = solution.Task.Lesson.Chapter.Course;

            if (course.Teacher.UserId != user.Id && solution.UserId != user.Id)
            {
                return Forbid();
            }

            return Ok(_mapper.Map<SolutionDto>(solution));
        }

        [Authorize(Roles="TEACHER")]
        [HttpGet("ReadByTaskId/{taskId}")]
        public async Task<ActionResult<IEnumerable<SolutionDto>>> GetByTask(int taskId)
        {
            var user = await _account.GetCurrentUserAsync();
            var task = await _tasks.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                return BadRequest();
            }

            var course = task.Lesson.Chapter.Course;

            if (course.Teacher.UserId != user.Id)
            {
                return Forbid();
            }

            var solutions = await _service.GetSolutionsByTaskIdAsync(taskId);
            return Ok(_mapper.Map<IEnumerable<SolutionDto>>(solutions));
        }

        [HttpGet("ReadByUserId/{taskId}")]
        public async Task<ActionResult<IEnumerable<SolutionDto>>> GetByUser(Guid userId)
        {
            var user = await _account.GetCurrentUserAsync();

            if (userId != user.Id)
            {
                return Forbid();
            }

            var solutions = await _service.GetSolutionsByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<SolutionDto>>(solutions));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSolution(SolutionCreationDto solution)
        {
            var user = await _account.GetCurrentUserAsync();
            var task = await _tasks.GetTaskByIdAsync(solution.TaskId);

            if (task == null)
            {
                return BadRequest();
            }

            var course = task.Lesson.Chapter.Course;
            if (!await _account.CheckAccessToCourse(course.Id, "SEND_TASK_SOLUTIONS_AND_GET_MARKS"))
            {
                return Forbid();
            }

            solution.UserId = user.Id;
            var _solution = _mapper.Map<Solution>(solution);
            return Created("Solution was created successfully",
                _mapper.Map<SolutionDto>(await _service.CreateSolutionAsync(_solution)));
        }

        [HttpPatch("UpdateLink/{solutionId}/{link}")]
        public async Task<IActionResult> UpdateLink(Guid solutionId, string link)
        {
            var user = await _account.GetCurrentUserAsync();
            var solution = await _service.GetSolutionByIdAsync(solutionId);

            if (solution == null)
            {
                return BadRequest();
            }

            if (user.Id != solution.UserId ||
                !await _account.CheckAccessToCourse(solution.Task.Lesson.Chapter.Course.Id,
                "HAVE_UNLIMITED_TRIES_TO_SEND_SOLUTIONS"))
            {
                return Forbid();
            }

            solution.Link = link;
            solution.AttemptNumber += 1;
           
            var _solution = _mapper.Map<Solution>(solution);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<SolutionDto>(await _service.UpdateSolutionAsync(_solution)));
        }

        [Authorize(Roles ="TEACHER")]
        [HttpPatch("LeaveComment/{solutionId}/{comment}")]
        public async Task<IActionResult> LeaveComment(Guid solutionId, string comment)
        {
            var user = await _account.GetCurrentUserAsync();
            var solution = await _service.GetSolutionByIdAsync(solutionId);

            if (solution == null)
            {
                return BadRequest();
            }

            var course = solution.Task.Lesson.Chapter.Course;

            if (course.Teacher.UserId != user.Id)
            {
                return Forbid();
            }

            solution.Comment = comment;

            var _solution = _mapper.Map<Solution>(solution);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<SolutionDto>(await _service.UpdateSolutionAsync(_solution)));
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPatch("RateSolution/{solutionId}/{mark}")]
        public async Task<IActionResult> RateSolution(Guid solutionId, double mark)
        {
            var user = await _account.GetCurrentUserAsync();
            var solution = await _service.GetSolutionByIdAsync(solutionId);

            if (solution == null)
            {
                return BadRequest();
            }

            var course = solution.Task.Lesson.Chapter.Course;

            if (course.Teacher.UserId != user.Id)
            {
                return Forbid();
            }

            solution.Mark = mark;

            var _solution = _mapper.Map<Solution>(solution);
            return StatusCode((int)HttpStatusCode.NoContent,
                _mapper.Map<SolutionDto>(await _service.UpdateSolutionAsync(_solution)));
        }
    }
}

