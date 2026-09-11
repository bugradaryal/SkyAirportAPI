using Business.Abstract;
using Business.Concrete;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.Personal.Queries;
using DTO;
using DTO.Account;
using DTO.Personal;
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
    [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public PersonalController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mediator = mediator;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetAllPersonals")]
        public async Task<IActionResult> GetAllPersonals()
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Personal>());
                return Ok(getAllRepository.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetAllPersonalsByAirportId")]
        public async Task<IActionResult> GetAllPersonalsByAirportId([FromQuery] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                var getAllResponse = await _mediator.Send(new GetAllPersonalByAirportIdRequest(id));
                return Ok(getAllResponse.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetPersonalById")]
        public async Task<IActionResult> GetPersonalById([FromQuery] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Personal>(id));
                return Ok(getByIdResponse.entity);
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Create)]
        [HttpPost("AddPersonal")]
        public async Task<IActionResult> AddPersonal([FromBody] PersonalAddDTO personalDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var personal = _mapper.Map<Personal, PersonalAddDTO>(personalDTO);
                await _mediator.Send(new GenericAddRequest<Personal>(personal));
                return Ok(new { message = "Personal added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Delete)]
        [HttpDelete("DeletePersonal")]
        public async Task<IActionResult> DeletePersonal([FromQuery] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<Personal>(id));
                return Ok(new { message = "Personal deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Update)]
        [HttpPut("UpdatePersonal")]
        public async Task<IActionResult> UpdatePersonal([FromBody] PersonalUpdateDTO personalDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var data = await _mediator.Send(new GenericGetByIdRequest<Personal>(personalDTO.id));
                var personal = _mapper.Map<Personal, PersonalUpdateDTO>(personalDTO, data.entity);
                await _mediator.Send(new GenericUpdateRequest<Personal>(personal));
                return Ok(new { message = "Personal Updated!!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}