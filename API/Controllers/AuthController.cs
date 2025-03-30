using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        public readonly IAccountServices _accountServices;
        private UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public AuthController(UserManager<User> userManager, IMapper mapper) 
        {
            _accountServices = new AccountManager(userManager,mapper);
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDTO createAccountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _accountServices.CreateAccount(createAccountDTO);
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("Login")]
        public async Task<IActionResult> LoginAccount([FromBody] CreateAccountDTO createAccountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _accountServices.CreateAccount(createAccountDTO);
            return Ok();
        }

        [Authorize]
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDTO createAccountDTO)
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
