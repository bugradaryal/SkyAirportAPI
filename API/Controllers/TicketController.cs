using System.Globalization;
using Business.Abstract;
using Business.Concrete;
using Business.Features.Airline.Queries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.OwnedTicket.Commands;
using Business.Features.OwnedTicket.Commands.AddOwnedTicket;
using Business.Features.OwnedTicket.Commands.DeleteOwnedTicket;
using Business.Features.OwnedTicket.Commands.UpdateOwnedTicket;
using Business.Redis;
using DTO;
using DTO.Account;
using DTO.OwnedTicket;
using DTO.Ticket;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        private readonly IRedisServices _redisServices;

        public TicketController(IMediator mediator, IMapper mapper, ITokenServices tokenServices, IRedisServices redisServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
            _redisServices = redisServices;
        }

        [AllowAnonymous]
        [HttpGet("GetAllTicket")]
        public async Task<IActionResult> GetAllTicket([FromQuery] string type = "TRY")
        {
            var getAllResponse = await _mediator.Send(new GenericGetAllRequest<Ticket>());
            if (type != "TRY" && string.IsNullOrEmpty(await _redisServices.GetAsync("forex")) != true)
            {
                var forex = await _redisServices.GetAsync("forex");
                if (!string.IsNullOrEmpty(forex))
                {
                    foreach (var item in getAllResponse.entity)
                    {
                        item.Price = item.Price * decimal.Parse(forex, CultureInfo.InvariantCulture);
                    }
                }
            }
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetTicketById")]
        public async Task<IActionResult> GetTicketById([FromQuery] int id, [FromQuery] string type = "TRY")
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GenericGetByIdRequest<Ticket>(id));
            if (type != "TRY")
            {
                var forex = await _redisServices.GetAsync("forex");
                if (!string.IsNullOrEmpty(forex))
                    getAllResponse.entity.Price = getAllResponse.entity.Price * decimal.Parse(forex, CultureInfo.InvariantCulture);          
            }
            return Ok(getAllResponse.entity);
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddTicket")]
        public async Task<IActionResult> AddTicket([FromBody] TicketAddDTO ticketAddDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var ticket = _mapper.Map<Ticket, TicketAddDTO>(ticketAddDTO);
                await _mediator.Send(new GenericAddRequest<Ticket>(ticket));
                return Ok(new { message = "Ticket added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteTicket")]
        public async Task<IActionResult> DeleteTicket([FromQuery] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<Ticket>(id));
                return Ok(new { message = "Ticket deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateTicket")]
        public async Task<IActionResult> UpdateTicket([FromBody] TicketUpdateDTO ticketUpdateDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var data = await _mediator.Send(new GenericGetByIdRequest<Ticket>(ticketUpdateDTO.id));
                var ticket = _mapper.Map<Ticket, TicketUpdateDTO>(ticketUpdateDTO, data.entity);
                await _mediator.Send(new GenericUpdateRequest<Ticket>(ticket));
                return Ok(new { message = "Ticket Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}