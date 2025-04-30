using Business.Features.Airline.Queries;
using Business.Features.Flight.Queries.GetAllFlightByAircraftId;
using Business.Features.Flight.Queries.GetAllFlightByAirlineId;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Flight;
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
    public class FlightController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FlightController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("GetAllFlight")]
        public async Task<IActionResult> GetAllFlight()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Flight>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllFlightByAirlineId")]
        public async Task<IActionResult> GetAllFlightByAirlineId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllFlightByAirlineIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }
        [AllowAnonymous]
        [HttpGet("GetAllFlightByAircraftId")]
        public async Task<IActionResult> GetAllFlightByAircraftId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllFlightByAircraftIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetFlightById")]
        public async Task<IActionResult> GetFlightById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Flight>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddFlight")]
        public async Task<IActionResult> AddFlight(FlightAddDTO flightDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var flight = _mapper.Map<Flight, FlightAddDTO>(flightDTO);
            flight.flight_Aircraft = new List<Flight_Aircraft> { new Flight_Aircraft { aircraft_id = flightDTO.aircraft_id } };
            var addResponse = await _mediator.Send(new GenericAddRequest<Flight>(flight));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Flight added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteFlight")]
        public async Task<IActionResult> DeleteFlight([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Flight>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Flight deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateFlight")]
        public async Task<IActionResult> UpdateFlight(FlightUpdateDTO flightDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Flight>(flightDTO.id));
            var flight = _mapper.Map<Flight, FlightUpdateDTO>(flightDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Flight>(flight));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            return Ok(new { message = "Updated!" });
        }

    }
}
