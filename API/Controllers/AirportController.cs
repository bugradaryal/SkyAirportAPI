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
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAirports()
        {
            return Ok(_genericServices.GetAll());
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAirportById([FromQuery]int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddAirport(AirportDTO airportDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(airportDTO);
            return Ok();
        }
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAirport([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateAirport(AirportDTO airportDTO)
        {
            return Ok(_dtogenericServices.Update(airportDTO));
        }
    }
}
