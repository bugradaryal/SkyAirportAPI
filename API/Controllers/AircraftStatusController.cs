using Business.Abstract;
using Business.Concrete;
using Business.Features.Aircraft.Queries.GetAircraftById;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.AircraftStatus;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftStatusController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public AircraftStatusController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _tokenServices = tokenServices;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllAircraftStatus")]
        public async Task<IActionResult> GetAllAircraftStatus()
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var getAllResponse = await _mediator.Send(new GenericGetAllRequest<AircraftStatus>());
                return Ok(getAllResponse.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [HttpGet("GetAircraftStatusById")]
        public async Task<IActionResult> GetAircraftStatusById([FromQuery] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });

                var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<AircraftStatus>(id));
                return Ok(getByIdResponse.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [HttpPost("AddAircraftStatus")]
        public async Task<IActionResult> AddAircraftStatus([FromBody]string newStatus)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                await _mediator.Send(new GenericAddRequest<AircraftStatus>(new AircraftStatus { Status = newStatus }));
                return Ok(new { message = "AircraftStatus added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [HttpDelete("DeleteAircraftStatus/{id}")]
        public async Task<IActionResult> DeleteAircraftStatus([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });

                await _mediator.Send(new GenericDeleteRequest<AircraftStatus>(id));
                return Ok(new { message = "AircraftStatus deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [HttpPut("UpdateAircraftStatus")]
        public async Task<IActionResult> UpdateAircraftStatus([FromBody]AircraftStatusUpdateDTO aircraftStatusDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var aircraftStatusResponse = await _mediator.Send(new GenericGetByIdRequest<AircraftStatus>(aircraftStatusDTO.id));
                var aircraftStatus = _mapper.Map<AircraftStatus, AircraftStatusUpdateDTO>(aircraftStatusDTO, aircraftStatusResponse.entity);
                await _mediator.Send(new GenericUpdateRequest<AircraftStatus>(aircraftStatus));

                return Ok(new { message = "AircraftStatus Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}
