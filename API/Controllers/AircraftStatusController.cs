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
    public class AircraftStatusController : ControllerBase
    {
        private readonly IGenericServices<AircraftStatus> _genericServices;
        private readonly IGenericServices<AircraftStatusDTO> _dtogenericServices;
        public AircraftStatusController() 
        {
            _genericServices = new GenericManager<AircraftStatus>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAircraftStatuss()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAircraftStatusById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddAircraftStatus(AircraftStatusDTO aircraftStatusDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(aircraftStatusDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAircraftStatus([FromQuery] int id)
        {
            return Ok(_genericServices.Delete(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> UpdateAircraftStatus(AircraftStatusDTO aircraftStatusDTO)
        {
            return Ok(_dtogenericServices.Update(aircraftStatusDTO));
        }
    }
}
