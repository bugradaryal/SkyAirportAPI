using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Business.Features.Account.Commands.ChangePassword;
using Business.Features.Account.Commands.CreateAccount;
using Business.Features.Account.Commands.DeleteAccount;
using Business.Features.Account.Commands.UpdateAccount;
using Business.Features.Account.Queries.GetUserRole;
using Business.Features.Account.Queries.Login;
using DTO;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using MediatR;
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
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AuthController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IOptions<JWT_Conf> jwt, IMediator mediator) 
        {
            _accountServices = new AccountManager(userManager,mapper,signInManager);
            _tokenServices = new TokenManager(jwt,userManager);
            _mapper = mapper;
            _mediator = mediator;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDTO createAccountDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });

            var createResponse = await _mediator.Send(new CreateAccountRequest(createAccountDTO));
            if(createResponse != null)
                return BadRequest(createResponse);
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
                var userResponse = await _mediator.Send(new LoginRequest(loginAccountDTO));
                if (userResponse.error == false)
                {
                    if (userResponse.user.IsSuspended)
                        return BadRequest(new { message = "User is suspended!!" });
                    var token = await _tokenServices.CreateTokenJWT(userResponse.user);
                    var roleResponse = await _mediator.Send(new GetUserRoleRequest(userResponse.user.Id));
                    if (roleResponse.error == false)
                    {
                        return Ok(new AuthenticationModel
                        {
                            Email = userResponse.user.Email,
                            Roles = roleResponse.UserRoles,
                            UserName = userResponse.user.UserName,
                            Token = token
                        });
                    }
                    return BadRequest(roleResponse.exception);
                }
                return BadRequest(userResponse.exception);

            }
            return Ok(_mapper.Map(validateTokenDTO, new AuthenticationModel()));         
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromQuery] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
                return Unauthorized(new { message = "Token not valid!!" });
            else
            {
                if(userId != validateTokenDTO.user.Id)
                    return BadRequest(new { message = "Unauthorized Action!!" });
                var deleteResponse = await _mediator.Send(new DeleteAccountRequest(validateTokenDTO.user));
                if (deleteResponse != null)
                    return BadRequest(deleteResponse);
                return Ok( new { message = "Account Deleted!!" });
            }

        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
                return Unauthorized(new { message = "Token not valid!!" });
            else
            {
                User user = _mapper.Map(updateAccountDTO,validateTokenDTO.user);
                var updateResponse = await _mediator.Send(new UpdateAccountRequest(user));
                if(updateResponse != null)
                    return BadRequest(updateResponse);
                return Ok(new { message = "Account Updated!!" });
            }
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
                return Unauthorized(new { message = "Token not valid!!" });
            else
            {
                var changePasswordResponse = await _mediator.Send(new ChangePasswordRequest(validateTokenDTO.user, changePasswordDTO));
                if(changePasswordResponse != null)
                    return BadRequest(changePasswordResponse);
                return Ok(new { message = "Password changed!!" });
            }
        }
    }
}
