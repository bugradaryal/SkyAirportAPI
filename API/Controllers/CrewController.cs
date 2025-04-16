using Business.Abstract;
using Business.Concrete.Generic;
using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly IGenericServices<Crew> _genericServices;
        private readonly IGenericServices<CrewDTO> _dtogenericServices;
        public CrewController() 
        {
            _genericServices = new GenericManager<Crew>();
            _dtogenericServices = new GenericManager<CrewDTO>();   
        }

        [AllowAnonymous]
        [HttpGet("GetAllCrews")]
        public async Task<IActionResult> GetAllCrews()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("GetCrewById")]
        public async Task<IActionResult> GetCrewById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddCrew")]
        public async Task<IActionResult> AddCrew(CrewDTO crewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(crewDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeleteCrew")]
        public async Task<IActionResult> DeleteCrew([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            await _genericServices.Delete(id);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("UpdateCrew")]
        public async Task<IActionResult> UpdateCrew(CrewDTO crewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Update(crewDTO);
            return Ok();
        }
    }
}
