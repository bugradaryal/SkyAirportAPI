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
    public class AirlineController : ControllerBase
    {
        private readonly IGenericServices<Airline> _genericServices;
        private readonly IGenericServices<AirlineDTO> _dtogenericServices;
        public AirlineController() 
        {
            _genericServices = new GenericManager<Airline>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAirlines()
        {
            return Ok(_genericServices.GetAll());
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAirlineById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddAirline(AirlineDTO airlineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(airlineDTO);
            return Ok();
        }
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAirline([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateAirline(AirlineDTO airlineDTO)
        {
            return Ok(_dtogenericServices.Update(airlineDTO));
        }
    }
}
