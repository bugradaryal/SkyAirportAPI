using Business.Abstract;
using Business.Concrete;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.AircraftStatus;
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
    [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftStatusController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerServices _logger;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public AircraftStatusController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllAircraftStatus")]
        public async Task<IActionResult> GetAllAircraftStatus()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAircraftStatus endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "AircraftStatus",
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
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<AircraftStatus>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "AircraftStatus",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAircraftStatus action done!",
                Action_type = Action_Type.Delete,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllRepository.data);
        }

        [HttpGet("GetAircraftStatusById")]
        public async Task<IActionResult> GetAircraftStatusById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAircraftStatusById endpoint called for {"+id??null+"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "AircraftStatus",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "AircraftStatus",
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

            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<AircraftStatus>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "AircraftStatus",
                    loglevel_id = getByIdResponse.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getByIdResponse.response.Exception);
                return BadRequest(getByIdResponse.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllAircraftStatusById action done for {"+id+"}",
                Action_type = Action_Type.APIResponse,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddAircraftStatus")]
        public async Task<IActionResult> AddAircraftStatus([FromBody]string newStatus)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddAircraftStatus endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "AircraftStatus",
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
            var aircraftStatus = new AircraftStatus { Status = newStatus };
            var addResponse = await _mediator.Send(new GenericAddRequest<AircraftStatus>(aircraftStatus));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "AircraftStatus",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }
                
            await _logger.Logger(new LogDTO
            {
                Message = "AddAircraftStatus action done!",
                Action_type = Action_Type.Create,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "AircraftStatus added!" });
        }

        [HttpDelete("DeleteAircraftStatus/{id}")]
        public async Task<IActionResult> DeleteAircraftStatus([FromRoute] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteAircraftStatus endpoint called for {"+id ?? null+"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "AircraftStatus",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "AircraftStatus",
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<AircraftStatus>(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "AircraftStatus",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteAircraftStatus action done for {"+id+"}",
                Action_type = Action_Type.Delete,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "AircraftStatus deleted!" });
        }

        [HttpPut("UpdateAircraftStatus")]
        public async Task<IActionResult> UpdateAircraftStatus([FromBody]AircraftStatusUpdateDTO aircraftStatusDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus endpoint called for {"+aircraftStatusDTO.id ?? null +"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "AircraftStatus",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<AircraftStatus>(aircraftStatusDTO.id));
            var aircraftStatus = _mapper.Map<AircraftStatus, AircraftStatusUpdateDTO>(aircraftStatusDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<AircraftStatus>(aircraftStatus));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "AircraftStatus",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }
                
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done for {"+aircraftStatusDTO.id+"}",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "AircraftStatus Updated!" });
        }
    }
}
