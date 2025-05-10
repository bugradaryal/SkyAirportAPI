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
                Message = "GetAllSeat endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "Seat",
                loglevel_id = 1,
            }, null);
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Seat>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Seat",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllSeat action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Seat",
                loglevel_id = 1
            }, null);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllSeatByFlightId")]
        public async Task<IActionResult> GetAllSeatByFlightId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllSeatByFlightId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Seat",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Seat",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getAllResponse = await _mediator.Send(new GetAllSeatByFlightIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Seat",
                    loglevel_id = getAllResponse.response.Exception.ExceptionLevel,
                }, getAllResponse.response.Exception);
                return BadRequest(getAllResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllSeatByFlightId action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Seat",
                loglevel_id = 1
            }, null);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetSeatById")]
        public async Task<IActionResult> GetSeatById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetSeatById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Seat",
                loglevel_id = 1,
            }, null);
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Seat",
                    loglevel_id = 3,
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Seat>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Seat",
                    loglevel_id = getByIdResponse.response.Exception.ExceptionLevel,
                }, getByIdResponse.response.Exception);
                return BadRequest(getByIdResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetSeatById action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Seat",
                loglevel_id = 1
            }, null);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddSeat")]
        public async Task<IActionResult> AddSeat(SeatAddDTO seatDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddSeat endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "Seat",
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
            var seat = _mapper.Map<Seat, SeatAddDTO>(seatDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Seat>(seat));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Sear added",
                Action_type = Action_Type.Create,
                Target_table = "Seat",
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
                Message = "DeleteSeat endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Seat",
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
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Seat",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Seat>(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Seat",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Seat deleted!",
                Action_type = Action_Type.Delete,
                Target_table = "Seat",
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
                Message = "UpdateSeat endpoint called for {" + seatDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Seat",
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
            var data = await _mediator.Send(new GenericGetByIdRequest<Seat>(seatDTO.id));
            var seat = _mapper.Map<Seat, SeatUpdateDTO>(seatDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Seat>(seat));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Seat Updated!",
                Action_type = Action_Type.Update,
                Target_table = "Seat",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Seat Updated!" });
        }
    }
}
