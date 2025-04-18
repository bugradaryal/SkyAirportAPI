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
    public class PersonalController : ControllerBase
    {
        private readonly IGenericServices<Personal> _genericServices;
        private readonly IGenericServices<PersonalDTO> _dtogenericServices;
        public PersonalController() 
        {
            _genericServices = new GenericManager<Personal>();
        }

        [AllowAnonymous]
        [HttpGet("GetAllPersonals")]
        public async Task<IActionResult> GetAllPersonals()
        {
            return Ok(_genericServices.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("GetPersonalById")]
        public async Task<IActionResult> GetPersonalById([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            return Ok(await _genericServices.GetValue(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddPersonal")]
        public async Task<IActionResult> AddPersonal(PersonalDTO personalDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Add(personalDTO);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeletePersonal")]
        public async Task<IActionResult> DeletePersonal([FromQuery] int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Invalid Id!!" });
            await _genericServices.Delete(id);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("UpdatePersonal")]
        public async Task<IActionResult> UpdatePersonal(PersonalDTO personalDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            await _dtogenericServices.Update(personalDTO);
            return Ok();
        }
    }
}
