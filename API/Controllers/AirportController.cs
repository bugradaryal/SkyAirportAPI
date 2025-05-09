using Business.Abstract;
using Business.Concrete;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Account;
using DTO.Airport;
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
    public class AirportController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AirportController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager) 
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }
        [AllowAnonymous]
        [HttpGet("GetAllAirports")]
        public async Task<IActionResult> GetAllAirports()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            var getAllResponse = await _mediator.Send(new GenericGetAllRequest<Airport>());
            if(getAllResponse.error)
                return BadRequest(getAllResponse.response.Exception);
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllResponse.data);
        }
        [AllowAnonymous]
        [HttpGet("GetAirportById")]
        public async Task<IActionResult> GetAirportById([FromQuery]int id)
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
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Airport>(id));
            if(getByIdResponse.error)
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
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAirport")]
        public async Task<IActionResult> AddAirport([FromBody] AirportAddDTO airportDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
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
            var airport = _mapper.Map<Airport, AirportAddDTO>(airportDTO);
            var addResponse =  await _mediator.Send(new GenericAddRequest<Airport>(airport));
            if(addResponse != null)
                return BadRequest(addResponse);
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Airport added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAirport/{id}")]
        public async Task<IActionResult> DeleteAirport([FromRoute] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Airport>(id));
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
            return Ok(new { message = "Airport deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAirport")]
        public async Task<IActionResult> UpdateAirport([FromBody] AirportUpdateDTO airportDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<Airport>(airportDTO.id));
            var airport = _mapper.Map<Airport, AirportUpdateDTO>(airportDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Airport>(airport));
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
