using Business.Abstract;
using Business.Concrete;
using Business.Features.Airline.Queries;
using Business.Features.Flight.Queries.GetAllFlightByAirlineId;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Account;
using DTO.Flight;
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
using Business.Features.Flight.Queries.GetAllFlightByAircraftId;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public FlightController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [AllowAnonymous]
        [HttpGet("GetAllFlight")]
        public async Task<IActionResult> GetAllFlight()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllFlight endpoint called",
                Action_type = Action_Type.APIRequest,
                Target_table = "Flight",
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
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Flight>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Flight",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllFlight action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Flight",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllFlightByAirlineId")]
        public async Task<IActionResult> GetAllFlightByAirlineId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllFlightByAirlineId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Flight",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Flight",
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
            var getAllResponse = await _mediator.Send(new GetAllFlightByAirlineIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Flight",
                    loglevel_id = getAllResponse.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getAllResponse.response.Exception);
                return BadRequest(getAllResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllFlightByAirlineId action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Flight",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllResponse.entity);
        }
        [AllowAnonymous]
        [HttpGet("GetAllFlightByAircraftId")]
        public async Task<IActionResult> GetAllFlightByAircraftId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllFlightByAircraftId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Flight",
                loglevel_id = 1,
            }, null);
            var getAllResponse = await _mediator.Send(new GetAllFlightByAircraftIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Flight",
                    loglevel_id = getAllResponse.response.Exception.ExceptionLevel
                }, getAllResponse.response.Exception);
                return BadRequest(getAllResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllFlightByAircraftId action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Flight",
                loglevel_id = 1
            }, null);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetFlightById")]
        public async Task<IActionResult> GetFlightById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetFlightById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Flight",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Flight",
                    loglevel_id = 3
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Flight>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Flight",
                    loglevel_id = getByIdResponse.response.Exception.ExceptionLevel
                }, getByIdResponse.response.Exception);
                return BadRequest(getByIdResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetFlightById action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Flight",
                loglevel_id = 1
            }, null);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddFlight")]
        public async Task<IActionResult> AddFlight(FlightAddDTO flightDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "Flight",
                loglevel_id = 1,
            }, null);
            var flight = _mapper.Map<Flight, FlightAddDTO>(flightDTO);
            flight.flight_Aircraft = new List<Flight_Aircraft> { new Flight_Aircraft { aircraft_id = flightDTO.aircraft_id } };
            var addResponse = await _mediator.Send(new GenericAddRequest<Flight>(flight));
            if (addResponse != null)
                return BadRequest(addResponse);
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Flight added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteFlight")]
        public async Task<IActionResult> DeleteFlight([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Flight>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Flight deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateFlight")]
        public async Task<IActionResult> UpdateFlight(FlightUpdateDTO flightDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            var data = await _mediator.Send(new GenericGetByIdRequest<Flight>(flightDTO.id));
            var flight = _mapper.Map<Flight, FlightUpdateDTO>(flightDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Flight>(flight));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Updated!" });
        }

    }
}
