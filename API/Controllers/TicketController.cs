using AutoMapper;
using Business.Abstract;
using Business.Features.Airline.Queries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllTicketBySeatId")]
        public async Task<IActionResult> GetAllTicketBySeatId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllAirlinesByAirportIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
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
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddTicket")]
        public async Task<IActionResult> AddTicket(TicketDTO ticketDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var ticket = _mapper.Map(ticketDTO, new Ticket());
            var addResponse = await _mediator.Send(new GenericAddRequest<Ticket>(ticket));
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Ticket>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Ticket deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateTicket")]
        public async Task<IActionResult> UpdateTicket(TicketDTO ticketDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Ticket>(ticketDTO.id));
            var ticket = _mapper.Map(ticketDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Ticket>(ticket));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            return Ok(new { message = "Updated!" });
        }
    }
}
