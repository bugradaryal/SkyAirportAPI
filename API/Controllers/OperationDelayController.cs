using AutoMapper;
using Business.Abstract;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.OperationalDelay.Queries;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationalDelayDTOController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OperationalDelayDTOController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllOperationalDelay")]
        public async Task<IActionResult> GetAllOperationalDelay()
        {
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<OperationalDelayDTO>());
            if (getAllRepository.error == true)
                return BadRequest(getAllRepository.exception);
            return Ok(getAllRepository.data);
        }


        [HttpGet("GetAllOperationalDelayByFlightId")]
        public async Task<IActionResult> GetAllOperationalDelayByFlightId([FromQuery] int id)
        {
            var getAllResponse = await _mediator.Send(new GetAllOperationalDelayByFlightIdRequest(id));
            if (getAllResponse.error)
                return BadRequest(getAllResponse.exception);
            return Ok(getAllResponse.entity);
        }

        [HttpGet("GetOperationalDelayById")]
        public async Task<IActionResult> GetOperationalDelayById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<OperationalDelayDTO>(id));
            if (getByIdResponse.error)
                return BadRequest(getByIdResponse.exception);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddOperationalDelay")]
        public async Task<IActionResult> AddOperationalDelay(OperationalDelayDTO operationDelayDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var personal = _mapper.Map(operationDelayDTO, new OperationalDelayDTO());
            var addResponse = await _mediator.Send(new GenericAddRequest<OperationalDelayDTO>(personal));
            if (addResponse != null)
                return BadRequest(addResponse);
            return Ok(new { message = "Delay added!" });
        }

        [HttpDelete("DeleteOperationalDelay")]
        public async Task<IActionResult> DeleteOperationalDelay([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<OperationalDelayDTO>(id));
            if (deleteResponse != null)
                return BadRequest(deleteResponse);
            return Ok(new { message = "OperationalDelay deleted!" });
        }

        [HttpPut("UpdateOperationalDelay")]
        public async Task<IActionResult> UpdateOperationalDelayDTO(OperationalDelayDTO operationDelayDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var data = await _mediator.Send(new GenericGetByIdRequest<OperationalDelayDTO>(operationDelayDTO.id));
            var personal = _mapper.Map(operationDelayDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<OperationalDelayDTO>(personal));
            return Ok(new { message = "Updated!" });
        }
    }
}
