using Business.Abstract;
using Business.Concrete.Generic;
using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationDelayController : ControllerBase
    {
        private readonly IGenericServices<OperationalDelay> _genericServices;
        private readonly IGenericServices<OperationalDelayDTO> _dtogenericServices;
        public OperationDelayController() 
        {
            _genericServices = new GenericManager<OperationalDelay>();
            _dtogenericServices = new GenericManager<OperationalDelayDTO>();    
        }

        [AllowAnonymous]
        [HttpGet("GetAllOperationDelays")]
        public async Task<IActionResult> GetAllOperationDelays()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("GetOperationDelayById")]
        public async Task<IActionResult> GetOperationDelayById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddOperationDelay")]
        public async Task<IActionResult> AddOperationDelay(DTO.OperationalDelayDTO operationDelayDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(operationDelayDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeleteOperationDelay")]
        public async Task<IActionResult> DeleteOperationDelay([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("UpdateOperationDelay")]
        public async Task<IActionResult> UpdateOperationDelay(DTO.OperationalDelayDTO operationDelayDTO)
        {
            return Ok(_dtogenericServices.Update(operationDelayDTO));
        }
    }
}
