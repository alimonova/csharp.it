using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly IAccountService _account;
        private readonly ITarifService _tarif;
        private readonly IMapper _mapper;

        public CourseController(ICourseService service, IMapper mapper,
            IAccountService account, ITarifService tarif)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _tarif = tarif;
        }

        [HttpGet("Read")]
        public async Task<ActionResult<IEnumerable<CourseWithTarifsDto>>> Get()
        {
            var courses = await _service.GetCoursesAsync();
            var coursesDto = _mapper.Map<IEnumerable<CourseWithTarifsDto>>(courses);

            return Ok(coursesDto);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> Get(int id, int currentLesson,
            int lessonsBefore = 1, int lessonsAfter = 1)
        {
            var watch = new Stopwatch();
            watch.Start();

            var course = await _service.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseDto = _mapper.Map<CourseDetailsDto>(course);

            if (!await _account.CheckAccessToCourse(id, "SEE_QUESTIONS"))
            {
                courseDto.Chapters.ForEach(x => x.Lessons.ForEach(l =>
                { l.Questions = new List<QuestionDetailsDto>(); }));
            }

            if (!await _account.CheckAccessToCourse(id, "SEE_ANSWERS_AND_CHECK_TEST"))
            {
                courseDto.Chapters.ForEach(x => x.Lessons.ForEach(l =>
                    { l.Questions.ForEach(q => { q.Answers =
                        new List<AnswerDto>(); }); }));
                courseDto.CheckTest = false;
            }

            if (!await _account.CheckAccessToCourse(id, "SEE_RIGHT_ANSWERS_EXPLANATIONS"))
            {
                courseDto.Chapters.ForEach(x => x.Lessons.ForEach(l =>
                { l.Questions.ForEach(q => { q.Explanation = ""; }); }));
            }

            if (!await _account.CheckAccessToCourse(id, "SEE_PRACTICAL_EXAMPLES"))
            {
                courseDto.Chapters.ForEach(x => x.Lessons.ForEach(l =>
                { l.PracticalExamples = new List<PracticalExampleDto>(); }));
            }

            if (!await _account.CheckAccessToCourse(id, "SEE_USEFUL_RESOURCES"))
            {
                courseDto.Chapters.ForEach(x => x.Lessons.ForEach(l =>
                { l.UsefulResources = new List<UsefulResourceDto>(); }));
            }

            if (!await _account.CheckAccessToCourse(id, "SEND_TASK_SOLUTIONS_AND_GET_MARKS"))
            {
                courseDto.SendTask = false;
            }

            if (!await _account.CheckAccessToCourse(id, "SEE_TASKS"))
            {
                courseDto.Chapters.ForEach(x => x.Lessons.ForEach(l =>
                { l.Tasks = new List<TaskDto>(); }));
            }

            watch.Stop();
            var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
            return Ok(courseDto);
        }

        [HttpGet("ReadByStudentId")]
        public async Task<ActionResult<IEnumerable<UserCourseDto>>> GetByStudent(Guid userId)
        {
            var roles = await _account.GetCurrentUserRoles();
            var user = await _account.GetCurrentUserAsync();


            if (!roles.ToList().Contains("ADMIN") && user.Id != userId)
            {
                return Forbid();
            }

            var userCourses = await _service.GetCoursesByStudentIdAsync(userId);
            var userCoursesDto = _mapper.Map<IEnumerable<UserCourseDto>>(userCourses);
            var courses = new List<Course>();

            courses.AddRange(userCourses.Select(x=>x.Tarif.Course));

            var coursesDto = _mapper.Map<IEnumerable<CourseDto>>(courses);

            for (int x = 0; x < coursesDto.Count(); x++)
            {
                var les = courses.ElementAt(x).Chapters.Select(x => x.Lessons.Count).Sum();
                coursesDto.ElementAt(x).LessonsNumber = les;

                userCoursesDto.ElementAt(x).Course = coursesDto.ElementAt(x);
            }

            return Ok(userCoursesDto);
        }

        [HttpGet("ReadByTeacherId/{teacherId}")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetByTeacher(int teacherId)
        {
            var channels = await _service.GetCoursesByTeacherIdAsync(teacherId);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(channels));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCourse(NewCourseDto course)
        {
            var user = await _account.GetCurrentUserAsync();

            var teacher = await _service.AddTeacherAsync(user.Id, course.AboutAuthor);

            return Created("Course was created successfully",
                _mapper.Map<CourseDto>(await _service.CreateCourseAsync(teacher, course)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateCourse(UpdateCourseDto model)
        {
            var user = await _account.GetCurrentUserAsync();
            var course = await _service.GetCourseByIdAsync(model.Id);

            if (course == null)
            {
                return BadRequest();
            }

            if (user.Id == course.Teacher.UserId)
            {
                return Ok(_mapper.Map<CourseDto>(
                    await _service.UpdateCourseAsync(course, model)));
            }

            return Forbid();
        }

        [HttpPatch("UpdateLesson")]
        public async Task<IActionResult> UpdateCourseLessonNum(int lessonNum, Guid tarifId, Guid userId)
        {
            var tarif = await _tarif.GetTarifByIdAsync(tarifId);
            var user = await _account.GetCurrentUserAsync();

            if (tarif == null)
            {
                return BadRequest();
            }

            if (user.Id == userId)
            {
                var userCourse = await _service.UpdateCourseLessonNumAsync(lessonNum, tarif, userId);

                if (userCourse == null)
                {
                    return BadRequest();
                }

                return Ok(_mapper.Map<UserCourseDto>(userCourse));
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
                return BadRequest();
            }

            if (user.Id == course.Teacher.UserId)
            {
                await _service.DeleteAsync(course);
                return StatusCode((int)HttpStatusCode.NoContent);
            }

            return Forbid();
        }
    }
}

