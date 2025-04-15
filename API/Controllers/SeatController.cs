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
    public class SeatController : ControllerBase
    {
        private readonly IGenericServices<Seat> _genericServices;
        private readonly IGenericServices<SeatDTO> _dtogenericServices;
        public SeatController() 
        {
            _genericServices = new GenericManager<Seat>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetSeatById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddSeat(SeatDTO seatDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(seatDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSeat([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateSeat(SeatDTO seatDTO)
        {
            return Ok(_dtogenericServices.Update(seatDTO));
        }
    }
}
