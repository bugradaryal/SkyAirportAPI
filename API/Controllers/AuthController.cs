using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using DTO;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        public readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AuthController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IOptions<JWT_Conf> jwt) 
        {
            _accountServices = new AccountManager(userManager,mapper,signInManager);
            _tokenServices = new TokenManager(jwt,userManager);
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDTO createAccountDTO) ///////////////////////
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });

            await _accountServices.CreateAccount(createAccountDTO);
            return Ok(new { message = "Account Created!" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAccount([FromBody] LoginAccountDTO loginAccountDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });

            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                User user = await _accountServices.LoginAccount(loginAccountDTO);
                if (user.IsSuspended)
                    return BadRequest(new { message = "User is suspended!!" });
                var token = _tokenServices.CreateTokenJWT(user);
                return Ok(new AuthenticationModel
                {
                    Email = user.Email,
                    Roles = await _accountServices.GetUserRoles(user.Id),
                    UserName = user.UserName,
                    Token = token
                });
            }
            return Ok(_mapper.Map<ValidateTokenDTO>(validateTokenDTO));         
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAccount/{userId}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
                return BadRequest(new { message = "Token not valid!!" });
            else
            {
                if(userId == validateTokenDTO.user.Id)
                    return BadRequest(new { message = "Unauthorized Action!!" });
                await _accountServices.DeleteAccount(validateTokenDTO.user);
                return Ok( new { message = "Account Deleted!!" });
            }

        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpDelete("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDTO updateAccountDTO) ///////////////////////
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
                return BadRequest(new { message = "Token not valid!!" });
            else
            {
                await _accountServices.UpdateAccount(updateAccountDTO);
                return Ok(new { message = "Account Updated!!" });
            }
        }
    }
}
