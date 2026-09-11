using Business.Abstract;
using Business.Features.Aircraft.Queries.GetAircraftById;
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetById;
using DTO.Aircraft;
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
    public class AircraftController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;
        public AircraftController(IMediator mediator, ITokenServices tokenServices, IMapper mapper)
        {
            _tokenServices = tokenServices;
            _mediator = mediator;
            _mapper = mapper;
        }
        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAllAircrafts")]
        public async Task<IActionResult> GetAllAircrafts()
        {
            var getAllResponse = await _mediator.Send(new GetAllAircraftsRequest());
            return Ok(getAllResponse.entity);
        }
        [LogAction(Action_Type.Read)]
        [AllowAnonymous]
        [HttpGet("GetAircraftById")]
        public async Task<IActionResult> GetAircraftById([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GetAircraftByIdRequest(id));
            return Ok(getByIdResponse.entity);
        }
        [LogAction(Action_Type.Create)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAircraft")]
        public async Task<IActionResult> AddAircraft([FromBody] AircraftAddDto aircraftDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                Aircraft aircraft = _mapper.Map<Aircraft, AircraftAddDto>(aircraftDTO);
                await _mediator.Send(new GenericAddRequest<Aircraft>(aircraft));
                return Ok(new { message = "Aircraft added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
        [LogAction(Action_Type.Delete)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAircraft/{id}")]
        public async Task<IActionResult> DeleteAircraft([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });

                await _mediator.Send(new GenericDeleteRequest<Aircraft>(id));
                return Ok(new { message = "Aircraft deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
        [LogAction(Action_Type.Update)]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAircraft")]
        public async Task<IActionResult> UpdateAircraft([FromBody] AircraftUpdateDTO aircraftDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var aircraftResponse = await _mediator.Send(new GenericGetByIdRequest<Aircraft>(aircraftDTO.id));
                var aircraft = _mapper.Map<Aircraft, AircraftUpdateDTO>(aircraftDTO, aircraftResponse.entity);
                await _mediator.Send(new GenericUpdateRequest<Aircraft>(aircraft));
                return Ok(new { message = "Aircraft Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}