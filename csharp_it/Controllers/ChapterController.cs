﻿using System;
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
	public class ChapterController : ControllerBase
    {
        private readonly IChapterService _service;
        private readonly IAccountService _account;
        private readonly ICourseService _courses;
        private readonly IMapper _mapper;

        public ChapterController(IChapterService service, IMapper mapper,
            IAccountService account, ICourseService courses)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _courses = courses;
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var chapter = await _service.GetChapterByIdAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ChapterDto>(chapter));
        }

        [HttpGet("ReadByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<ChapterDto>>> GetByCourse(int courseId)
        {
            var chapters = await _service.GetChaptersByCourseIdAsync(courseId);
            return Ok(_mapper.Map<IEnumerable<ChapterDto>>(chapters));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateChapter(ChapterDto chapter)
        {
            var user = await _account.GetCurrentUserAsync();
            var course = await _courses.GetCourseByIdAsync(chapter.CourseId);
            if (user.Id != course.Teacher.UserId)
            {
                return Forbid();
            }

            var _chapter = _mapper.Map<Chapter>(chapter);
            return Created("Chapter was created successfully", _mapper.Map<ChapterDto>(await _service.CreateChapterAsync(_chapter)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateChapter(ChapterDto chapter)
        {
            var user = await _account.GetCurrentUserAsync();
            var course = await _courses.GetCourseByIdAsync(chapter.CourseId);
            if (user.Id != course.Teacher.UserId)
            {
                return Forbid();
            }

            var _chapter = _mapper.Map<Chapter>(chapter);
            return StatusCode((int)HttpStatusCode.NoContent, _mapper.Map<ChapterDto>(await _service.UpdateChapterAsync(_chapter)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _account.GetCurrentUserAsync();
            var chapter = await _service.GetChapterByIdAsync(id);

            if (chapter == null)
            {
                return BadRequest();
            }

            var course = await _courses.GetCourseByIdAsync(chapter.CourseId);
            if (user.Id != course.Teacher.UserId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(chapter);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

