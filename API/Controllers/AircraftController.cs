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
    public class AircraftController : ControllerBase
    {
        private readonly IGenericServices<Aircraft> _genericServices;
        private readonly IGenericServices<AircraftDTO> _dtogenericServices;

        public AircraftController() 
        {
            _genericServices = new GenericManager<Aircraft>();
            _dtogenericServices = new GenericManager<AircraftDTO>();
        }

        [AllowAnonymous]
        [HttpGet("GetAllAircrafts")]
        public async Task<IActionResult> GetAllAircrafts()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("GetAircraftById")]
        public async Task<IActionResult> GetAircraftById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddAircraft")]
        public async Task<IActionResult> AddAircraft(AircraftDTO aircraftDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(aircraftDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeleteAircraft")]
        public async Task<IActionResult> DeleteAircraft([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("UpdateAircraft")]
        public async Task<IActionResult> UpdateAircraft(AircraftDTO aircraftDTO)
        {
            return Ok(_dtogenericServices.Update(aircraftDTO));
        }
    }
}
