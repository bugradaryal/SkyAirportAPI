using Business.Abstract;
using Business.Features.Flight.Queries.GetAllFlightByAircraftId;
using Business.Features.Flight.Queries.GetAllFlightByAirlineId;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO.Flight;
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
    public class FlightController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public FlightController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllFlight")]
        public async Task<IActionResult> GetAllFlight()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Flight>());
            return Ok(getAllRepository.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllFlightByAirlineId/{id}")]
        public async Task<IActionResult> GetAllFlightByAirlineId([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GetAllFlightByAirlineIdRequest(id));
            return Ok(getAllResponse.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllFlightByAircraftId/{id}")]
        public async Task<IActionResult> GetAllFlightByAircraftId([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GetAllFlightByAircraftIdRequest(id));
            return Ok(getAllResponse.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetFlightById/{id}")]
        public async Task<IActionResult> GetFlightById([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Flight>(id));
            return Ok(getByIdResponse.entity);
        }

        [LogAction(Action_Type.Create)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddFlight")]
        public async Task<IActionResult> AddFlight([FromBody] FlightAddDTO flightDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var flight = _mapper.Map<Flight, FlightAddDTO>(flightDTO);
                flight.flight_Aircraft = new List<Flight_Aircraft> { new Flight_Aircraft { aircraft_id = flightDTO.aircraft_id } };
                await _mediator.Send(new GenericAddRequest<Flight>(flight));
                return Ok(new { message = "Flight added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Delete)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteFlight/{id}")]
        public async Task<IActionResult> DeleteFlight([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<Flight>(id));
                return Ok(new { message = "Flight deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Update)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateFlight")]
        public async Task<IActionResult> UpdateFlight([FromBody] FlightUpdateDTO flightDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var data = await _mediator.Send(new GenericGetByIdRequest<Flight>(flightDTO.id));
                var flight = _mapper.Map<Flight, FlightUpdateDTO>(flightDTO, data.entity);
                await _mediator.Send(new GenericUpdateRequest<Flight>(flight));
                return Ok(new { message = "Flight Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}