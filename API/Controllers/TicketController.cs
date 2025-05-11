using Business.Abstract;
using Business.Concrete;
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
using DTO.Account;
using DTO.Ticket;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public TicketController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [AllowAnonymous]
        [HttpGet("GetAllTicket")]
        public async Task<IActionResult> GetAllTicket()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllTicket endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "Ticket",
                loglevel_id = 1,
            }, null);
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Ticket>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Ticket",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllTicket action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Ticket",
                loglevel_id = 1
            }, null);
            return Ok(getAllRepository.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetAllTicketBySeatId")]
        public async Task<IActionResult> GetAllTicketBySeatId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllTicketBySeatId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Ticket",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getAllResponse = await _mediator.Send(new GetAllAirlinesByAirportIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = getAllResponse.response?.Exception?.ExceptionLevel,
                }, getAllResponse.response?.Exception);
                return BadRequest(getAllResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllTicketBySeatId action done for {"+id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "Ticket",
                loglevel_id = 1
            }, null);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetTicketById")]
        public async Task<IActionResult> GetTicketById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetTicketById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Ticket",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Ticket>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = getByIdResponse.response?.Exception?.ExceptionLevel,
                }, getByIdResponse.response?.Exception);
                return BadRequest(getByIdResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetTicketById action done for {"+id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "Ticket",
                loglevel_id = 1
            }, null);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddTicket")]
        public async Task<IActionResult> AddTicket(TicketAddDTO ticketDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddTicket endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "Ticket",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            var ticket = _mapper.Map<Ticket, TicketAddDTO>(ticketDTO);
            var addResponse = await _mediator.Send(new AddTicketRequest(ticket));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Ticket added!",
                Action_type = Action_Type.Create,
                Target_table = "Ticket",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Ticket added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteTicket")]
        public async Task<IActionResult> DeleteTicket([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteTicket endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Ticket",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = 3
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            var deleteResponse = await _mediator.Send(new DeleteTicketRequest(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Ticket deleted for {"+id+"}",
                Action_type = Action_Type.Delete,
                Target_table = "Ticket",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Ticket deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateTicket")]
        public async Task<IActionResult> UpdateTicket(TicketUpdateDTO ticketDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateTicket endpoint called for {" + ticketDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Ticket",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            var data = await _mediator.Send(new GenericGetByIdRequest<Ticket>(ticketDTO.id));
            var ticket = _mapper.Map<Ticket, TicketUpdateDTO>(ticketDTO, data.entity);
            var updateResponse = await _mediator.Send(new UpdateTicketRequest(ticket));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Ticket",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Ticket Updated for {"+ticketDTO.id+"}",
                Action_type = Action_Type.Update,
                Target_table = "Ticket",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Ticket Updated!" });
        }
    }
}
