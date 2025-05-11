using Business.Abstract;
using Business.Concrete;
using Business.Features.Aircraft.Queries.GetAircraftById;
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Aircraft;
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
    public class AircraftController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AircraftController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _tokenServices = new TokenManager(jwt,userManager);
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpGet("GetAllAircrafts")]
        public async Task<IActionResult> GetAllAircrafts()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAircrafts endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            var getAllResponse = await _mediator.Send(new GetAllAircraftsRequest());
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Aircraft",
                    loglevel_id = getAllResponse.response?.Exception?.ExceptionLevel,
                }, getAllResponse.response?.Exception);
                return BadRequest(getAllResponse.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAircraft action done!",
                Action_type = Action_Type.Read,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            return Ok(getAllResponse.entity);
        }
        [AllowAnonymous]
        [HttpGet("GetAircraftById")]
        public async Task<IActionResult> GetAircraftById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAircraftById endpoint called for {"+id??null+"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Aircraft",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
                
            var getByIdResponse = await _mediator.Send(new GetAircraftByIdRequest(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Aircraft",
                    loglevel_id = getByIdResponse.response?.Exception?.ExceptionLevel,
                }, getByIdResponse.response?.Exception);
                return BadRequest(getByIdResponse.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetByIdAircraft action done for {"+id+"}",
                Action_type = Action_Type.Read,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAircraft")]
        public async Task<IActionResult> AddAircraft([FromBody]AircraftAddDto aircraftDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddAircraft endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
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
            var aircraft = _mapper.Map<Aircraft, AircraftAddDto>(aircraftDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Aircraft>(aircraft));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Aircraft",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Aircraft added!",
                Action_type = Action_Type.Create,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id= validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Aircraft added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAircraft/{id}")]
        public async Task<IActionResult> DeleteAircraft([FromRoute] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteAircraft endpoint called for {"+id ?? null+"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid id value!!.",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Aircraft",
                    loglevel_id = 1
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Aircraft>(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Aircraft",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Aircraft deleted for {"+id+"}",
                Action_type = Action_Type.Delete,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Aircraft deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAircraft")]
        public async Task<IActionResult> UpdateAircraft([FromBody]AircraftUpdateDTO aircraftDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {"+aircraftDTO.id ?? null +"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<Aircraft>(aircraftDTO.id));
            var aircraft = _mapper.Map<Aircraft, AircraftUpdateDTO>(aircraftDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Aircraft>(aircraft));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Aircraft",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Aircraft updated for {"+aircraftDTO.id+"}",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Aircraft Updated!" });
        }
    }
}
