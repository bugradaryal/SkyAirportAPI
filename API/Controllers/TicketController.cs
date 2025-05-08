using Business.Abstract;
using Business.Features.Airline.Queries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.Ticket.Commands;
using Business.Features.Ticket.Commands.AddTicket;
using Business.Features.Ticket.Commands.DeleteTicket;
using Business.Features.Ticket.Commands.UpdateTicket;
using DTO;
using DTO.Ticket;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TicketController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("GetAllTicket")]
        public async Task<IActionResult> GetAllTicket()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Ticket>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.response.Exception);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllTicketBySeatId")]
        public async Task<IActionResult> GetAllTicketBySeatId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllAirlinesByAirportIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.response.Exception);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetTicketById")]
        public async Task<IActionResult> GetTicketById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Ticket>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.response.Exception);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddTicket")]
        public async Task<IActionResult> AddTicket(TicketAddDTO ticketDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var ticket = _mapper.Map<Ticket, TicketAddDTO>(ticketDTO);
            var addResponse = await _mediator.Send(new AddTicketRequest(ticket));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Ticket added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteTicket")]
        public async Task<IActionResult> DeleteTicket([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new DeleteTicketRequest(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Ticket deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateTicket")]
        public async Task<IActionResult> UpdateTicket(TicketUpdateDTO ticketDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Ticket>(ticketDTO.id));
            var ticket = _mapper.Map<Ticket, TicketUpdateDTO>(ticketDTO, data.entity);
            var updateResponse = await _mediator.Send(new UpdateTicketRequest(ticket));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            return Ok(new { message = "Updated!" });
        }
    }
}
