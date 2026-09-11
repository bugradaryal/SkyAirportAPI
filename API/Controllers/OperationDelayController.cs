using Business.Abstract;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.OperationalDelay.Queries;
using DTO.OperationalDelay;
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
    public class OperationalDelayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public OperationalDelayController(IMediator mediator, IMapper mapper, ITokenServices tokenServices)
        {
            _mediator = mediator;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetAllOperationalDelay")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOperationalDelay()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<OperationalDelay>());
            return Ok(getAllRepository.entity);
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetAllOperationalDelayByFlightId/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOperationalDelayByFlightId([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getAllResponse = await _mediator.Send(new GetAllOperationalDelayByFlightIdRequest(id));
            return Ok(getAllResponse.entity);
        }

        [LogAction(Action_Type.Read)]
        [HttpGet("GetOperationalDelayById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOperationalDelayById([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<OperationalDelay>(id));
            return Ok(getByIdResponse.entity);
        }

        [LogAction(Action_Type.Create)]
        [HttpPost("AddOperationalDelay")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> AddOperationalDelay([FromBody] OperationalDelayAddDTO operationDelayDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var personal = _mapper.Map<OperationalDelay, OperationalDelayAddDTO>(operationDelayDTO);
                await _mediator.Send(new GenericAddRequest<OperationalDelay>(personal));
                return Ok(new { message = "Delay added!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Delete)]
        [HttpDelete("DeleteOperationalDelay/{id}")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> DeleteOperationalDelay([FromRoute] int id)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Id!!" });
                await _mediator.Send(new GenericDeleteRequest<OperationalDelay>(id));
                return Ok(new { message = "OperationalDelay deleted!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [LogAction(Action_Type.Update)]
        [HttpPut("UpdateOperationalDelay")]
        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        public async Task<IActionResult> UpdateOperationalDelayDTO([FromBody] OperationalDelayUpdateDTO operationDelayDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var data = await _mediator.Send(new GenericGetByIdRequest<OperationalDelay>(operationDelayDTO.id));
                var personal = _mapper.Map<OperationalDelay, OperationalDelayUpdateDTO>(operationDelayDTO, data.entity);
                await _mediator.Send(new GenericUpdateRequest<OperationalDelay>(personal));
                return Ok(new { message = "OperationalDelay Updated!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}