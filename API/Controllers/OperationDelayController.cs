using Business.Abstract;
using Business.Concrete;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.OperationalDelay.Queries;
using DTO;
using DTO.Account;
using DTO.OperationalDelay;
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
    public class OperationalDelayController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public OperationalDelayController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [HttpGet("GetAllOperationalDelay")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOperationalDelay()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOperationalDelay endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "OperationalDelay",
                loglevel_id = 1,
            }, null);
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<OperationalDelay>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOperationalDelay action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "OperationalDelay",
                loglevel_id = 1
            }, null);
            return Ok(getAllRepository.data);
        }


        [HttpGet("GetAllOperationalDelayByFlightId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOperationalDelayByFlightId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOperationalDelayByFlightId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OperationalDelay",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = 3
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getAllResponse = await _mediator.Send(new GetAllOperationalDelayByFlightIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = getAllResponse.response.Exception.ExceptionLevel,
                }, getAllResponse.response.Exception);
                return BadRequest(getAllResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllOperationalDelayByFlightId action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "OperationalDelay",
                loglevel_id = 1
            }, null);
            return Ok(getAllResponse.entity);
        }

        [HttpGet("GetOperationalDelayById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOperationalDelayById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetOperationalDelayById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OperationalDelay",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<OperationalDelay>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = getByIdResponse.response.Exception.ExceptionLevel,
                }, getByIdResponse.response.Exception);
                return BadRequest(getByIdResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetOperationalDelayById action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "OperationalDelay",
                loglevel_id = 1
            }, null);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddOperationalDelay")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> AddOperationalDelay(OperationalDelayAddDTO operationDelayDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddOperationalDelay endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "OperationalDelay",
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
            var personal = _mapper.Map<OperationalDelay, OperationalDelayAddDTO>(operationDelayDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<OperationalDelay>(personal));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "OperationalDelay added!",
                Action_type = Action_Type.Create,
                Target_table = "OperationalDelay",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Delay added!" });
        }

        [HttpDelete("DeleteOperationalDelay")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> DeleteOperationalDelay([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OperationalDelay",
                loglevel_id = 1,
            }, null);

            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<OperationalDelay>(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "OperationalDelay deleted!",
                Action_type = Action_Type.Delete,
                Target_table = "OperationalDelay",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "OperationalDelay deleted!" });
        }

        [HttpPut("UpdateOperationalDelay")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> UpdateOperationalDelayDTO(OperationalDelayUpdateDTO operationDelayDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + operationDelayDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "OperationalDelay",
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
            }
            var data = await _mediator.Send(new GenericGetByIdRequest<OperationalDelay>(operationDelayDTO.id));
            var personal = _mapper.Map<OperationalDelay, OperationalDelayUpdateDTO>(operationDelayDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<OperationalDelay>(personal));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "OperationalDelay",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "OperationalDelay updated!",
                Action_type = Action_Type.Update,
                Target_table = "OperationalDelay",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "OperationalDelay Updated!" });
        }
    }
}
