using Business.Abstract;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.AircraftStatus;
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
    public class AircraftStatusController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AircraftStatusController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllAircraftStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAircraftStatus()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<AircraftStatus>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }

        [HttpGet("GetAircraftStatusById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAircraftStatusById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<AircraftStatus>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddAircraftStatus")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> AddAircraftStatus([FromBody]string newStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var aircraftStatus = new AircraftStatus { Status = newStatus };
            var addResponse = await _mediator.Send(new GenericAddRequest<AircraftStatus>(aircraftStatus));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "AircraftStatus added!" });
        }

        [HttpDelete("DeleteAircraftStatus/{id}")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> DeleteAircraftStatus([FromRoute] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<AircraftStatus>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "AircraftStatus deleted!" });
        }

        [HttpPut("UpdateAircraftStatus")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> UpdateAircraftStatus([FromBody]AircraftStatusUpdateDTO aircraftStatusDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<AircraftStatus>(aircraftStatusDTO.id));
            var aircraftStatus = _mapper.Map<AircraftStatus, AircraftStatusUpdateDTO>(aircraftStatusDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<AircraftStatus>(aircraftStatus));
            if(updateResponse != null)
            {
                return BadRequest(updateResponse);
            }
            return Ok(new { message = "Updated!" });
        }
    }
}
