using Business.Abstract;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.Seat.Queries;
using DTO.Seat;
using Entities;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilitys.Logging;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public SeatController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllSeat")]
        public async Task<IActionResult> GetAllSeat()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Seat>());
            return Ok(getAllRepository.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllSeatByFlightId/{id}")]
        public async Task<IActionResult> GetAllSeatByFlightId([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GetAllSeatTicketRequest(id));
            return Ok(getAllResponse.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetSeatById/{id}  ")]
        public async Task<IActionResult> GetSeatById([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Seat>(id));
            return Ok(getByIdResponse.entity);
        }

        [LogAction(Action_Type.Create)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddSeat")]
        public async Task<IActionResult> AddSeat([FromBody] SeatAddDTO seatDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var seat = _mapper.Map<Seat, SeatAddDTO>(seatDTO);
                await _mediator.Send(new GenericAddRequest<Seat>(seat));
                return Ok(new { message = "Seat added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Delete)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteSeat/{id}")]
        public async Task<IActionResult> DeleteSeat([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<Seat>(id));
                return Ok(new { message = "Seat deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Update)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateSeat")]
        public async Task<IActionResult> UpdateSeat([FromBody] SeatUpdateDTO seatDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var data = await _mediator.Send(new GenericGetByIdRequest<Seat>(seatDTO.id));
                var seat = _mapper.Map<Seat, SeatUpdateDTO>(seatDTO, data.entity);
                await _mediator.Send(new GenericUpdateRequest<Seat>(seat));
                return Ok(new { message = "Seat Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}