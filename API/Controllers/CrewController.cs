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
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCrews()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCrewById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddCrew(CrewDTO crewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(crewDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCrew([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateCrew(CrewDTO crewDTO)
        {
            return Ok(_dtogenericServices.Update(crewDTO));
        }
    }
}
