using Business.Abstract;
using Business.Concrete;
using Business.Features.Crew.Qeeries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Account;
using DTO.Crew;
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
    public class CrewController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public CrewController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [HttpGet("GetAllCrew")]
        public async Task<IActionResult> GetAllCrew()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllCrew endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "Crew",
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
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Crew>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Crew",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllCrew action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Crew",
                loglevel_id = 1,
                user_id= validateTokenDTO.user.Id
            }, null);
            return Ok(getAllRepository.data);
        }

        [HttpGet("GetAllCrewByAircraftId")]
        public async Task<IActionResult> GetAllCrewByAircraftId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllCrewByAircraftId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Crew",
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
            var getAllResponse = await _mediator.Send(new GetAllCrewByAircraftIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Crew",
                    loglevel_id = getAllResponse.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getAllResponse.response.Exception);
                return BadRequest(getAllResponse.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllCrewByAircraftId action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Crew",
                loglevel_id = 1,
                user_id=validateTokenDTO.user.Id
            }, null);
            return Ok(getAllResponse.entity);
        }

        [HttpGet("GetCrewById")]
        public async Task<IActionResult> GetCrewById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetCrewById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Crew",
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
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Crew",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Crew>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Crew",
                    loglevel_id = getByIdResponse.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getByIdResponse.response.Exception);
                return BadRequest(getByIdResponse.response);
            }
                
            await _logger.Logger(new LogDTO
            {
                Message = "GetCrewById action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Crew",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddCrew")]
        public async Task<IActionResult> AddCrew(CrewAddDTO crewDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "Crew",
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
            var crew = _mapper.Map<Crew, CrewAddDTO>(crewDTO);
            crew.crew_Aircraft = new List<Crew_Aircraft> { new Crew_Aircraft { aircraft_id = crewDTO.aircraft_id } };
            var addResponse = await _mediator.Send(new GenericAddRequest<Crew>(crew));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Crew",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Crew added!",
                Action_type = Action_Type.Create,
                Target_table = "Crew",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Crew added!" });
        }

        [HttpDelete("DeleteCrew")]
        public async Task<IActionResult> DeleteCrew([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteCrew endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Crew",
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
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Crew",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Crew>(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Crew",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Crew deleted!",
                Action_type = Action_Type.Delete,
                Target_table = "Crew",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Crew deleted!" });
        }

        [HttpPut("UpdateCrew")]
        public async Task<IActionResult> UpdateCrew(CrewUpdateDTO crewDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateCrew endpoint called for {" + crewDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Crew",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<Crew>(crewDTO.id));
            var crew = _mapper.Map<Crew, CrewUpdateDTO>(crewDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Crew>(crew));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Crew",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id= validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Crew updated!",
                Action_type = Action_Type.Update,
                Target_table = "Crew",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Updated!" });
        }
    }
}
