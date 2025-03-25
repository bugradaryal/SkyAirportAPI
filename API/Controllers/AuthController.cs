using Business.Abstract;
using Business.Concrete;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        public readonly IAccountServices _accountServices;
        public AuthController() 
        {
            _accountServices = new AccountManager();
        }





        [AllowAnonymous]
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDTO createAccountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _accountServices.CreateAccount(createAccountDTO);
            return Ok();
        }
    }
}
