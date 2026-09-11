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
using Business.Features.OwnedTicket.Queries;
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
using Utilitys.Logging;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnedTicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public OwnedTicketController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllOwnedTicket")]
        public async Task<IActionResult> GetAllOwnedTicket()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<OwnedTicket>());
            return Ok(getAllRepository.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllOwnedTicketBySeatId")]
        public async Task<IActionResult> GetAllOwnedTicketBySeatId([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GetAllOwnedTicketBySeatIdRequest(id));
            return Ok(getAllResponse.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetOwnedTicketById")]
        public async Task<IActionResult> GetOwnedTicketById([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<OwnedTicket>(id));
            return Ok(getByIdResponse.entity);
        }

        [LogAction(Action_Type.Create)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddOwnedTicket")]
        public async Task<IActionResult> AddOwnedTicket([FromBody] OwnedTicketAddDTO OwnedTicketDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var OwnedTicket = _mapper.Map<OwnedTicket, OwnedTicketAddDTO>(OwnedTicketDTO);
                await _mediator.Send(new AddOwnedTicketRequest(OwnedTicket));
                return Ok(new { message = "OwnedTicket added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Delete)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteOwnedTicket")]
        public async Task<IActionResult> DeleteOwnedTicket([FromQuery] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new DeleteOwnedTicketRequest(id));
                return Ok(new { message = "OwnedTicket deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Update)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateOwnedTicket")]
        public async Task<IActionResult> UpdateOwnedTicket([FromBody] OwnedTicketUpdateDTO OwnedTicketDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var data = await _mediator.Send(new GenericGetByIdRequest<OwnedTicket>(OwnedTicketDTO.id));
                var OwnedTicket = _mapper.Map<OwnedTicket, OwnedTicketUpdateDTO>(OwnedTicketDTO, data.entity);
                await _mediator.Send(new UpdateOwnedTicketRequest(OwnedTicket));
                return Ok(new { message = "OwnedTicket Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}