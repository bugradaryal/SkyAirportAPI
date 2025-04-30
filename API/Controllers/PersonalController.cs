using Business.Abstract;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.Personal.Queries;
using DTO;
using DTO.Personal;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Authorize(Roles = "Administrator",Policy = "IsUserSuspended")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PersonalController(IMediator mediator, IMapper mapper) 
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllPersonals")]
        public async Task<IActionResult> GetAllPersonals()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Personal>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }


        [HttpGet("GetAllPersonalsByAirportId")]
        public async Task<IActionResult> GetAllPersonalsByAirportId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllPersonalByAirportIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }

        [HttpGet("GetPersonalById")]
        public async Task<IActionResult> GetPersonalById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Personal>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddPersonal")]
        public async Task<IActionResult> AddPersonal(PersonalAddDTO personalDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var personal = _mapper.Map<Personal, PersonalAddDTO>(personalDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Personal>(personal));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Personal added!" });
        }

        [HttpDelete("DeletePersonal")]
        public async Task<IActionResult> DeletePersonal([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Personal>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "Personal deleted!" });
        }

        [HttpPut("UpdatePersonal")]
        public async Task<IActionResult> UpdatePersonal(PersonalUpdateDTO personalDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<Personal>(personalDTO.id));
            var personal = _mapper.Map<Personal, PersonalUpdateDTO>(personalDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Personal>(personal));
            return Ok(new { message = "Updated!" });
        }
    }
}
