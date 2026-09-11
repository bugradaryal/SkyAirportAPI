using Business.Abstract;
using Business.Concrete;
using Business.Features.Airline.Queries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Account;
using DTO.Airline;
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
    public class AirlineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AirlineController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllAirlines")]
        public async Task<IActionResult> GetAllAirlines()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Airline>());
            return Ok(getAllRepository.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllAirlinesByAirportId")]
        public async Task<IActionResult> GetAllAirlinesByAirportId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllAirlinesByAirportIdRequest(id));
            return Ok(getAllResponse.entity);
        }

        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAirlineById")]
        public async Task<IActionResult> GetAirlineById([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Airline>(id));
            return Ok(getByIdResponse.entity);
        }
        [LogAction(Action_Type.Create)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAirline")]
        public async Task<IActionResult> AddAirline([FromBody] AirlineAddDTO airlineDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                Airline airline = _mapper.Map<Airline, AirlineAddDTO>(airlineDTO);
                await _mediator.Send(new GenericAddRequest<Airline>(airline));
                return Ok(new { message = "Airline added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
        [LogAction(Action_Type.Delete)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAirline/{id}")]
        public async Task<IActionResult> DeleteAirline([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<Airline>(id));
                return Ok(new { message = "Airline deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
        [LogAction(Action_Type.Update)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAirline")]
        public async Task<IActionResult> UpdateAirline([FromBody] AirlineUpdateDTO airlineDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var airlineResponse = await _mediator.Send(new GenericGetByIdRequest<Airline>(airlineDTO.id));
                var airline = _mapper.Map<Airline, AirlineUpdateDTO>(airlineDTO, airlineResponse.entity);
                await _mediator.Send(new GenericUpdateRequest<Airline>(airline));
                return Ok(new { message = "Airline Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}