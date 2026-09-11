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
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public FlightController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
        }

        [AllowAnonymous]
        [HttpGet("GetAllFlight")]
        public async Task<IActionResult> GetAllFlight()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Flight>());
            return Ok(getAllRepository.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetAllFlightByAirlineId")]
        public async Task<IActionResult> GetAllFlightByAirlineId([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GetAllFlightByAirlineIdRequest(id));
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetAllFlightByAircraftId")]
        public async Task<IActionResult> GetAllFlightByAircraftId([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GetAllFlightByAircraftIdRequest(id));
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetFlightById")]
        public async Task<IActionResult> GetFlightById([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Flight>(id));
            return Ok(getByIdResponse.entity);
        }

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

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteFlight")]
        public async Task<IActionResult> DeleteFlight([FromQuery] int id)
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