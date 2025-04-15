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
    public class TicketController : ControllerBase
    {
        private readonly IGenericServices<Ticket> _genericServices;
        private readonly IGenericServices<TicketDTO> _dtogenericServices;
        public TicketController() 
        {
            _genericServices = new GenericManager<Ticket>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTicketById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddTicket(TicketDTO ticketDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(ticketDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTicket([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateTicket(TicketDTO ticketDTO)
        {
            return Ok(_dtogenericServices.Update(ticketDTO));
        }
    }
}
