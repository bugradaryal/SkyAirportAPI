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
    public class AirportController : ControllerBase
    {
        private readonly IGenericServices<Airport> _genericServices;
        private readonly IGenericServices<AirportDTO> _dtogenericServices;
        public AirportController() 
        {
            _genericServices = new GenericManager<Airport>();
            _dtogenericServices = new GenericManager<AirportDTO>();
        }
        [AllowAnonymous]
        [HttpGet("GetAllAirports")]
        public async Task<IActionResult> GetAllAirports()
        {
            return Ok(_genericServices.GetAll());
        }
        [AllowAnonymous]
        [HttpGet("GetAirportById")]
        public async Task<IActionResult> GetAirportById([FromQuery]int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost("AddAirport")]
        public async Task<IActionResult> AddAirport(AirportDTO airportDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(airportDTO);
            return Ok();
        }
        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeleteAirport")]
        public async Task<IActionResult> DeleteAirport([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            await _genericServices.Delete(id);
            return Ok();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut("UpdateAirport")]
        public async Task<IActionResult> UpdateAirport(AirportDTO airportDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Update(airportDTO);
            return Ok();
        }
    }
}
