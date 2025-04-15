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
    public class Crew_AircraftController : ControllerBase
    {
        private readonly IGenericServices<Crew_Aircraft> _genericServices;
        private readonly IGenericServices<Crew_AircraftDTO> _dtogenericServices;
        public Crew_AircraftController() 
        {
            _genericServices = new GenericManager<Crew_Aircraft>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCrew_Aircrafts()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCrew_AircraftById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddCrew_Aircraft(Crew_AircraftDTO crew_AircraftDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(crew_AircraftDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCrew_Aircraft([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateCrew_Aircraft(Crew_AircraftDTO crew_AircraftDTO)
        {
            return Ok(_dtogenericServices.Update(crew_AircraftDTO));
        }
    }
}
