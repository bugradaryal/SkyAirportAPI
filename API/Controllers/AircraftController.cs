using Business.Abstract;
using Business.Features.Aircraft.Queries.GetAircraftById;
using Business.Features.Aircraft.Queries.GetAllAircrafts;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Aircraft;
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
    public class AircraftController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public AircraftController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("GetAllAircrafts")]
        public async Task<IActionResult> GetAllAircrafts()
        {
            var getAllResponse = await _mediator.Send(new GetAllAircraftsRequest());
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }
        [AllowAnonymous]
        [HttpGet("GetAircraftById")]
        public async Task<IActionResult> GetAircraftById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GetAircraftByIdRequest(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddAircraft")]
        public async Task<IActionResult> AddAircraft([FromBody]AircraftAddDto aircraftDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var aircraft = _mapper.Map<Aircraft, AircraftAddDto>(aircraftDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Aircraft>(aircraft));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Aircraft added!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAircraft/{id}")]
        public async Task<IActionResult> DeleteAircraft([FromRoute] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Aircraft>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Aircraft deleted!" });
        }
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPut("UpdateAircraft")]
        public async Task<IActionResult> UpdateAircraft([FromBody]AircraftUpdateDTO aircraftDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Aircraft>(aircraftDTO.id));
            var aircraft = _mapper.Map<Aircraft, AircraftUpdateDTO>(aircraftDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Aircraft>(aircraft));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            return Ok(new { message = "Updated!" });
        }
    }
}
