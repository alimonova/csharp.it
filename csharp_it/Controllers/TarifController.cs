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
    public class TarifController : ControllerBase
    {
        private readonly ITarifService _service;
        private readonly IAccountService _account;
        private readonly ICourseService _courses;
        private readonly IAccessService _accesses;
        private readonly IMapper _mapper;

        public TarifController(ITarifService service, IMapper mapper,
            IAccountService account, ICourseService courses, IAccessService accesses)
        {
            _service = service;
            _mapper = mapper;
            _account = account;
            _courses = courses;
            _accesses = accesses;
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var tarif = await _service.GetTarifByIdAsync(id);

            if (tarif == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TarifDto>(tarif));
        }

        [HttpGet("ReadByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<TarifDto>>> GetByCourse(int courseId)
        {
            var course = await _courses.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                return BadRequest();
            }

            var tarifs = await _service.GetTarifsByCourseIdAsync(courseId);
            var tarifsDto = _mapper.Map<IEnumerable<TarifDetailsDto>>(tarifs);

            return Ok(tarifsDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTarif(TarifDto tarif)
        {
            var user = await _account.GetCurrentUserAsync();
            var course = await _courses.GetCourseByIdAsync(tarif.CourseId);

            if (course == null)
            {
                return BadRequest();
            }

            if (user.Id != course.Teacher.UserId)
            {
                return Forbid();
            }

            var _tarif = await _service.CreateTarifAsync(_mapper.Map<Tarif>(tarif));

            await this._service.CreateTarifAccessesAsync(_tarif.Id, tarif.Accesses);

            return Created("Tarif was created successfully",
                _mapper.Map<TarifDto>(_tarif));
        }

        [HttpPost("AddAccessToTarif/{tarifId}/{accessId}")]
        public async Task<IActionResult> AddAccessToTarif(Guid tarifId, int accessId)
        {
            var user = await _account.GetCurrentUserAsync();
            var tarif = await _service.GetTarifByIdAsync(tarifId);
            var access = await _accesses.GetAccessByIdAsync(accessId);

            if (tarif == null || access == null) {
                return BadRequest();
            }

            var course = tarif.Course;

            if (user.Id != course.Teacher.UserId)
            {
                return Forbid();
            }

            await _service.AddAccessToTarifAsync(tarifId, accessId);
            return Ok("TarifAccess was created successfully");
        }

        [HttpDelete("RemoveAccessFromTarif/{tarifId}/{accessId}")]
        public async Task<IActionResult> RemoveAccessFromTarif(Guid tarifId, int accessId)
        {
            var user = await _account.GetCurrentUserAsync();
            var tarifAccess = await _service.GetTarifAccessAsync(tarifId, accessId);

            var tarif = await _service.GetTarifByIdAsync(tarifId);

            if (tarif == null || tarifAccess == null)
            {
                return BadRequest();
            }

            if (user.Id != tarif.Course.Teacher.UserId)
            {
                return Forbid();
            }

            await _service.RemoveAccessFromTarifAsync(tarifAccess);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _account.GetCurrentUserAsync();
            var tarif = await _service.GetTarifByIdAsync(id);

            if (tarif == null)
            {
                return BadRequest();
            }

            if (user.Id != tarif.Course.Teacher.UserId)
            {
                return Forbid();
            }

            await _service.DeleteAsync(tarif);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

