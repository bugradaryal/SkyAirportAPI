using AutoMapper;
using Business.Abstract;
using Business.Concrete.Generic;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IGenericServices<Airport> _genericServices;
        private readonly IGenericServices<AirportDTO> _dtogenericServices;
        public AirportController(IMediator mediator, IMapper mapper) 
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("GetAllAirports")]
        public async Task<IActionResult> GetAllAirports()
        {
            return Ok(_genericServices.GetAll());
        }
        [AllowAnonymous]
        [HttpGet("GetAirportById")]
        public async Task<IActionResult> GetAirportById([FromQuery]int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost("AddAirport")]
        public async Task<IActionResult> AddAirport(AirportDTO airportDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var airport = _mapper.Map(airportDTO, new Airport());
            var addResponse =  await _mediator.Send(new GenericAddRequest<Airport>(airport));
            if(addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Airport added!" });
        }
        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeleteAirport")]
        public async Task<IActionResult> DeleteAirport([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Airport deleted!" });
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut("UpdateAirport")]
        public async Task<IActionResult> UpdateAirport(AirportDTO airportDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var airport = _mapper.Map(airportDTO,);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest());
            return Ok();
        }
    }
}
