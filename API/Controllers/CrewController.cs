using Business.Abstract;
using Business.Features.Crew.Qeeries;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Crew;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public CrewController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("GetAllCrew")]
        public async Task<IActionResult> GetAllCrew()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Crew>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }

        [HttpGet("GetAllCrewByAircraftId")]
        public async Task<IActionResult> GetAllCrewByAircraftId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllCrewByAircraftIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }

        [HttpGet("GetCrewById")]
        public async Task<IActionResult> GetCrewById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Crew>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddCrew")]
        public async Task<IActionResult> AddCrew(CrewAddDTO crewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var crew = _mapper.Map<Crew, CrewAddDTO>(crewDTO);
            crew.crew_Aircraft = new List<Crew_Aircraft> { new Crew_Aircraft { aircraft_id = crewDTO.aircraft_id } };
            var addResponse = await _mediator.Send(new GenericAddRequest<Crew>(crew));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Crew added!" });
        }

        [HttpDelete("DeleteCrew")]
        public async Task<IActionResult> DeleteCrew([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Crew>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Crew deleted!" });
        }

        [HttpPut("UpdateCrew")]
        public async Task<IActionResult> UpdateCrew(CrewUpdateDTO crewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Crew>(crewDTO.id));
            var crew = _mapper.Map<Crew, CrewUpdateDTO>(crewDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Crew>(crew));
            if (updateResponse != null)
                return BadRequest(updateResponse);
            return Ok(new { message = "Updated!" });
        }
    }
}
