using Business.Abstract;
using Business.Features.Airline.Queries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SeatController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("GetAllSeat")]
        public async Task<IActionResult> GetAllSeat()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Seat>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllSeatByFlightId")]
        public async Task<IActionResult> GetAllSeatByFlightId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllAirlinesByAirportIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetSeatById")]
        public async Task<IActionResult> GetSeatById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Seat>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddSeat")]
        public async Task<IActionResult> AddSeat(SeatDTO seatDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var seat = _mapper.Map<Seat,SeatDTO>(seatDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Seat>(seat));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Seat added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteSeat")]
        public async Task<IActionResult> DeleteSeat([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Seat>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Seat deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateSeat")]
        public async Task<IActionResult> UpdateSeat(SeatDTO seatDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Seat>(seatDTO.id));
            var seat = _mapper.Map<Seat,SeatDTO>(seatDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Seat>(seat));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            return Ok(new { message = "Updated!" });
        }
    }
}
