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
using Microsoft.AspNetCore.Mvc;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirlineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AirlineController(IMediator mediator, IMapper mapper) 
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("GetAllAirlines")]
        public async Task<IActionResult> GetAllAirlines()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Airline>());
            if(getAllRepository.error == true)
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }

        [AllowAnonymous]
        [HttpGet("GetAllAirlinesByAirportId")]
        public async Task<IActionResult> GetAllAirlinesByAirportId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllAirlinesByAirportIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }

        [AllowAnonymous]
        [HttpGet("GetAirlineById")]
        public async Task<IActionResult> GetAirlineById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Airline>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAirline")]
        public async Task<IActionResult> AddAirline(AirlineDTO airlineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var airline = _mapper.Map<Airline,AirlineDTO>(airlineDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Airline>(airline));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Airline added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAirline")]
        public async Task<IActionResult> DeleteAirline([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Airline>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Airline deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAirline")]
        public async Task<IActionResult> UpdateAirline(AirlineDTO airlineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Airline>(airlineDTO.id));
            var airline = _mapper.Map<Airline,AirlineDTO>(airlineDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Airline>(airline));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            return Ok(new { message = "Updated!" });
        }
    }
}
