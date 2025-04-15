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
        private readonly IGenericServices<OperationDelay> _genericServices;
        private readonly IGenericServices<OperationDelayDTO> _dtogenericServices;
        public OperationDelayController() 
        {
            _genericServices = new GenericManager<OperationDelay>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllOperationDelays()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetOperationDelayById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddOperationDelay(OperationDelayDTO operationDelayDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(operationDelayDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOperationDelay([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateOperationDelay(OperationDelayDTO operationDelayDTO)
        {
            return Ok(_dtogenericServices.Update(operationDelayDTO));
        }
    }
}
