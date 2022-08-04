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
	[Authorize(Roles="ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAccessService _service;
        private readonly IMapper _mapper;

        public AccessController(IAccessService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Get()
        {
            var accesses = await _service.GetAccesses();

            if (accesses == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<AccessDto>>(accesses));
        }

        [HttpGet("ReadById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var access = await _service.GetAccessByIdAsync(id);

            if (access == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AccessDto>(access));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAccess(AccessDto access)
        {
            var _access = _mapper.Map<Access>(access);
            return Created("Access was created successfully", _mapper.Map<AccessDto>(await _service.CreateAccessAsync(_access)));
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateAccess(AccessDto access)
        {
            var _access = _mapper.Map<Access>(access);
            return StatusCode((int)HttpStatusCode.NoContent, _mapper.Map<AccessDto>(await _service.UpdateAccessAsync(_access)));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var access = await _service.GetAccessByIdAsync(id);

            if (access == null)
            {
                return BadRequest();
            }

            await _service.DeleteAsync(access);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}

