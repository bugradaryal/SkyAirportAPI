using Business.Abstract;
using Business.Features.Crew.Qeeries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO.Crew;
using Entities;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilitys.Logging;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public CrewController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetAllCrew")]
        public async Task<IActionResult> GetAllCrew()
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Crew>());
                return Ok(getAllRepository.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetAllCrewByAircraftId/{id}")]
        public async Task<IActionResult> GetAllCrewByAircraftId([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                var getAllResponse = await _mediator.Send(new GetAllCrewByAircraftIdRequest(id));
                return Ok(getAllResponse.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetCrewById/{id}")]
        public async Task<IActionResult> GetCrewById([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Crew>(id));
                return Ok(getByIdResponse.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Create)]
        [HttpPost("AddCrew")]
        public async Task<IActionResult> AddCrew([FromBody] CrewAddDTO crewDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var crew = _mapper.Map<Crew, CrewAddDTO>(crewDTO);
                crew.crew_Aircraft = new List<Crew_Aircraft> { new Crew_Aircraft { aircraft_id = crewDTO.aircraft_id } };
                await _mediator.Send(new GenericAddRequest<Crew>(crew));
                return Ok(new { message = "Crew added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Delete)]
        [HttpDelete("DeleteCrew/{id}")]
        public async Task<IActionResult> DeleteCrew([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<Crew>(id));
                return Ok(new { message = "Crew deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Update)]
        [HttpPut("UpdateCrew")]
        public async Task<IActionResult> UpdateCrew([FromBody] CrewUpdateDTO crewDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var data = await _mediator.Send(new GenericGetByIdRequest<Crew>(crewDTO.id));
                var crew = _mapper.Map<Crew, CrewUpdateDTO>(crewDTO, data.entity);
                await _mediator.Send(new GenericUpdateRequest<Crew>(crew));
                return Ok(new { message = "Crew Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}