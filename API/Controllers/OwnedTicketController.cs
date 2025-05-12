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
using DTO;
using DTO.Account;
using DTO.OwnedTicket;
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
    public class OwnedOwnedTicketController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public OwnedOwnedTicketController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [AllowAnonymous]
        [HttpGet("GetAllOwnedTicket")]
        public async Task<IActionResult> GetAllOwnedTicket()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOwnedTicket endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "OwnedTicket",
                loglevel_id = 1,
            }, null);
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<OwnedTicket>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "OwnedTicket",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOwnedTicket action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "OwnedTicket",
                loglevel_id = 1
            }, null);
            return Ok(getAllRepository.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetAllOwnedTicketBySeatId")]
        public async Task<IActionResult> GetAllOwnedTicketBySeatId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOwnedTicketBySeatId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OwnedTicket",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OwnedTicket",
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
                    Target_table = "OwnedTicket",
                    loglevel_id = getAllResponse.response?.Exception?.ExceptionLevel,
                }, getAllResponse.response?.Exception);
                return BadRequest(getAllResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOwnedTicketBySeatId action done for {"+id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "OwnedTicket",
                loglevel_id = 1
            }, null);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetOwnedTicketById")]
        public async Task<IActionResult> GetOwnedTicketById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetOwnedTicketById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OwnedTicket",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OwnedTicket",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<OwnedTicket>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OwnedTicket",
                    loglevel_id = getByIdResponse.response?.Exception?.ExceptionLevel,
                }, getByIdResponse.response?.Exception);
                return BadRequest(getByIdResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetOwnedTicketById action done for {"+id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "OwnedTicket",
                loglevel_id = 1
            }, null);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddOwnedTicket")]
        public async Task<IActionResult> AddOwnedTicket(OwnedTicketAddDTO OwnedTicketDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddOwnedTicket endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "OwnedTicket",
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
            var OwnedTicket = _mapper.Map<OwnedTicket, OwnedTicketAddDTO>(OwnedTicketDTO);
            var addResponse = await _mediator.Send(new AddOwnedTicketRequest(OwnedTicket));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OwnedTicket",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "OwnedTicket added!",
                Action_type = Action_Type.Create,
                Target_table = "OwnedTicket",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "OwnedTicket added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteOwnedTicket")]
        public async Task<IActionResult> DeleteOwnedTicket([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteOwnedTicket endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OwnedTicket",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OwnedTicket",
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
            var deleteResponse = await _mediator.Send(new DeleteOwnedTicketRequest(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OwnedTicket",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "OwnedTicket deleted for {"+id+"}",
                Action_type = Action_Type.Delete,
                Target_table = "OwnedTicket",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "OwnedTicket deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateOwnedTicket")]
        public async Task<IActionResult> UpdateOwnedTicket(OwnedTicketUpdateDTO OwnedTicketDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateOwnedTicket endpoint called for {" + OwnedTicketDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OwnedTicket",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<OwnedTicket>(OwnedTicketDTO.id));
            var OwnedTicket = _mapper.Map<OwnedTicket, OwnedTicketUpdateDTO>(OwnedTicketDTO, data.entity);
            var updateResponse = await _mediator.Send(new UpdateOwnedTicketRequest(OwnedTicket));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OwnedTicket",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "OwnedTicket Updated for {"+OwnedTicketDTO.id+"}",
                Action_type = Action_Type.Update,
                Target_table = "OwnedTicket",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "OwnedTicket Updated!" });
        }
    }
}
