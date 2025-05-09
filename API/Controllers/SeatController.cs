using Business.Abstract;
using Business.Concrete;
using Business.Features.Airline.Queries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.Seat.Queries;
using DTO;
using DTO.Account;
using DTO.Seat;
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
    public class SeatController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public SeatController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [AllowAnonymous]
        [HttpGet("GetAllSeat")]
        public async Task<IActionResult> GetAllSeat()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Seat>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.response.Exception);
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllSeatByFlightId")]
        public async Task<IActionResult> GetAllSeatByFlightId([FromQuery] int id)
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
            var getAllResponse = await _mediator.Send(new GetAllSeatByFlightIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.response.Exception);
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraftStatus action done!",
                Action_type = Action_Type.Update,
                Target_table = "Aircraft",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetSeatById")]
        public async Task<IActionResult> GetSeatById([FromQuery] int id)
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
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Seat>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.response.Exception);
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
        [HttpPost("AddSeat")]
        public async Task<IActionResult> AddSeat(SeatAddDTO seatDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            var seat = _mapper.Map<Seat, SeatAddDTO>(seatDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Seat>(seat));
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
            return Ok(new { message = "Seat added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteSeat")]
        public async Task<IActionResult> DeleteSeat([FromQuery] int id)
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
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Seat>(id));
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
            return Ok(new { message = "Seat deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateSeat")]
        public async Task<IActionResult> UpdateSeat(SeatUpdateDTO seatDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAircraft endpoint called for {" + aircraftDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Aircraft",
                loglevel_id = 1,
            }, null);
            var data = await _mediator.Send(new GenericGetByIdRequest<Seat>(seatDTO.id));
            var seat = _mapper.Map<Seat, SeatUpdateDTO>(seatDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Seat>(seat));
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
