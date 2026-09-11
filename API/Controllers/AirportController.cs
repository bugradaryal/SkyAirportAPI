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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilitys.Logging;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AirportController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllAirports")]
        public async Task<IActionResult> GetAllAirports()
        {
            var getAllResponse = await _mediator.Send(new GenericGetAllRequest<Airport>());
            return Ok(getAllResponse.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAirportById")]
        public async Task<IActionResult> GetAirportById([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Airport>(id));
            return Ok(getByIdResponse.entity);
        }

        [LogAction(Action_Type.Create)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAirport")]
        public async Task<IActionResult> AddAirport([FromBody] AirportAddDTO airportDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                Airport airport = _mapper.Map<Airport, AirportAddDTO>(airportDTO);
                await _mediator.Send(new GenericAddRequest<Airport>(airport));
                return Ok(new { message = "Airport added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Delete)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAirport/{id}")]
        public async Task<IActionResult> DeleteAirport([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<Airport>(id));
                return Ok(new { message = "Airport deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Update)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAirport")]
        public async Task<IActionResult> UpdateAirport([FromBody] AirportUpdateDTO airportDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var airportResponse = await _mediator.Send(new GenericGetByIdRequest<Airport>(airportDTO.id));
                var airport = _mapper.Map<Airport, AirportUpdateDTO>(airportDTO, airportResponse.entity);
                await _mediator.Send(new GenericUpdateRequest<Airport>(airport));
                return Ok(new { message = "Airport Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}