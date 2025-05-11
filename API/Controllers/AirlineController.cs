using Business.Abstract;
using Business.Concrete;
using Business.Features.Airline.Queries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Account;
using DTO.Airline;
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
    public class AirlineController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AirlineController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager) 
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [AllowAnonymous]
        [HttpGet("GetAllAirlines")]
        public async Task<IActionResult> GetAllAirlines()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAirlines endpoint called",
                Action_type = Action_Type.APIRequest,
                Target_table = "Airlines",
                loglevel_id = 1,
            }, null);
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Airline>());
            if(getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "AircraftStatus",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAirlines action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Airlines",
                loglevel_id = 1
            }, null);
            return Ok(getAllRepository.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetAllAirlinesByAirportId")]
        public async Task<IActionResult> GetAllAirlinesByAirportId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAirlinesByAirportId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Airlines",
                loglevel_id = 1,
            }, null);
            var getAllResponse = await _mediator.Send(new GetAllAirlinesByAirportIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Airlines",
                    loglevel_id = getAllResponse.response?.Exception?.ExceptionLevel,
                }, getAllResponse.response?.Exception);
                return BadRequest(getAllResponse.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAirlinesByAirportId action done for {"+id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "Airlines",
                loglevel_id = 1
            }, null);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetAirlineById")]
        public async Task<IActionResult> GetAirlineById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAirlineById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Airlines",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Airlines",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Airline>(id));
            if(getByIdResponse.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Airlines",
                    loglevel_id = getByIdResponse.response?.Exception?.ExceptionLevel,
                }, getByIdResponse.response?.Exception);
                return BadRequest(getByIdResponse.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetAirlineById action done for {"+id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "Airlines",
                loglevel_id = 1
            }, null);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAirline")]
        public async Task<IActionResult> AddAirline([FromBody]AirlineAddDTO airlineDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddAirline endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "Airlines",
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
            var airline = _mapper.Map<Airline, AirlineAddDTO>(airlineDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Airline>(airline));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Airlines",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }
                
            await _logger.Logger(new LogDTO
            {
                Message = "Airline added!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Airlines",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Airline added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAirline/{id}")]
        public async Task<IActionResult> DeleteAirline([FromRoute] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteAirline endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIResponse,
                Target_table = "Airlines",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Airlines",
                    loglevel_id = 3,
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Airline>(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Airlines",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Airline deleted for {"+id+"}",
                Action_type = Action_Type.Delete,
                Target_table = "Airlines",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Airline deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAirline")]
        public async Task<IActionResult> UpdateAirline([FromBody]AirlineUpdateDTO airlineDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAirline endpoint called for {" + airlineDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Airlines",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<Airline>(airlineDTO.id));
            var airline = _mapper.Map<Airline, AirlineUpdateDTO>(airlineDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Airline>(airline));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Airlines",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Airline Updated for {"+airlineDTO.id+"}",
                Action_type = Action_Type.Update,
                Target_table = "Airlines",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Airline Updated!" });
        }
    }
}
