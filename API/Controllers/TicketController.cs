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
        private readonly IRedisServices _redisServices;

        public TicketController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager, IRedisServices redisServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
            _redisServices = redisServices;
        }

        [AllowAnonymous]
        [HttpGet("GetAllTicket")]
        public async Task<IActionResult> GetAllTicket([FromQuery]string type = "TRY")
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllTicket endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "Ticket",
                loglevel_id = 1,
            }, null);
            var getAllResponse = await _mediator.Send(new GenericGetAllRequest<Ticket>());
            if (getAllResponse.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Ticket",
                    loglevel_id = getAllResponse.response.Exception.ExceptionLevel,
                }, getAllResponse.response.Exception);
                return BadRequest(getAllResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllTicket action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "T",
                loglevel_id = 1
            }, null);
            if(type != "TRY")
            {
                foreach(var item in getAllResponse.entity)
                {
                    item.Price = item.Price * decimal.Parse(await _redisServices.GetAsync("forex"),CultureInfo.InvariantCulture);
                }
            }
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetTicketById")]
        public async Task<IActionResult> GetTicketById([FromQuery] int id, [FromQuery] string type = "TRY")
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
            var getAllResponse = await _mediator.Send(new GenericGetByIdRequest<Ticket>(id));
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
            if (type != "TRY")
                getAllResponse.entity.Price = getAllResponse.entity.Price * decimal.Parse(await _redisServices.GetAsync("forex"), CultureInfo.InvariantCulture);
  
            await _logger.Logger(new LogDTO
            {
                Message = "GetTicketById action done for {" + id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "Ticket",
                loglevel_id = 1
            }, null);
            return Ok(getAllResponse.entity);
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddTicket")]
        public async Task<IActionResult> AddTicket([FromBody]TicketAddDTO ticketAddDTO)
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
            var ticket = _mapper.Map<Ticket, TicketAddDTO>(ticketAddDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Ticket>(ticket));
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Ticket>(id));
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
        public async Task<IActionResult> UpdateTicket([FromBody]TicketUpdateDTO ticketUpdateDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateTicket endpoint called for {" + ticketUpdateDTO.id ?? null + "}",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<Ticket>(ticketUpdateDTO.id));
            var ticket = _mapper.Map<Ticket, TicketUpdateDTO>(ticketUpdateDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Ticket>(ticket));
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
                Message = "Ticket Updated for {"+ticketUpdateDTO.id+"}",
                Action_type = Action_Type.Update,
                Target_table = "Ticket",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Ticket Updated!" });
        }
    }
}
