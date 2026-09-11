using Business.Abstract;
using Business.Concrete;
using Business.Features.Account.Commands.ChangePassword;
using Business.Features.Account.Commands.CreateAccount;
using Business.Features.Account.Commands.DeleteAccount;
using Business.Features.Account.Commands.UpdateAccount;
using Business.Features.Account.Commands.AddRoleToUser;
using Business.Features.Account.Commands.RemoveRoleFromUser;
using Business.Features.Account.Queries.GetUserByEmail;
using Business.Features.Account.Queries.GetUserRole;
using Business.Features.Account.Queries.Login;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetById;
using DTO;
using DTO.Account;
using DTO.Airport;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using System;
using System.ComponentModel.DataAnnotations;
using Utilitys.MailServices;
using Utilitys.Mapper;
using Business.Features.Account.Commands.SuspendUser;
using Business.Features.Account.Queries.GetUserById;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITokenServices _tokenServices;
        private readonly IMailServices _mailServices;
        private readonly IPhoneServices _phoneServices;
        private readonly string _callBackURL;
        public AccountController(IMediator mediator, IOptions<CallBackURL> callBackURL,
            ITokenServices tokenServices, IMailServices mailServices, IPhoneServices phoneServices) 
        {
            _tokenServices = tokenServices;
            _mediator = mediator;
            _mailServices = mailServices;
            _phoneServices = phoneServices;
            _callBackURL = callBackURL.Value.URL;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDTO createAccountDTO)
        {
            await _mediator.Send(new CreateAccountRequest(createAccountDTO));
            return Ok(new { message = "Account Created!" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAccount([FromBody] LoginAccountDTO loginAccountDTO)
        {
            var userId = User.FindFirst("uid")?.Value;
            GetUserRoleResponse roleResponse;
            User user;
            string? token = null;
            string? refreshToken = null;
            if (!string.IsNullOrEmpty(userId))
            {
                var userResponse = await _mediator.Send(new GetUserByIdRequest(userId));
                user = userResponse.user;
                if (user.IsSuspended)
                    return Unauthorized("User Suspended!!");
            }
            else
            {
                var loginResponse = await _mediator.Send(new LoginRequest(loginAccountDTO));
                user = loginResponse.user;
                token = await _tokenServices.CreateTokenJWT(user);
                refreshToken = _tokenServices.GenerateRefreshToken();
                await _tokenServices.SaveRefreshTokenAsync(user, refreshToken);
            }
            roleResponse = await _mediator.Send(new GetUserRoleRequest(user.Id));
            return Ok(new AuthenticationModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Roles = roleResponse.UserRoles,
                JwtToken = token,
                RefreshToken = refreshToken
            });
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAccount/{userId}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string userId)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                if (tokenUserId != userId)
                    return Unauthorized("Unauthorized Access!!");
                var userResponse = await _mediator.Send(new GetUserByIdRequest(userId));
                await _mediator.Send(new DeleteAccountRequest(userResponse.user));
                return Ok(new { message = "Account Deleted!!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                await _mediator.Send(new UpdateAccountRequest(updateAccountDTO, tokenUserId));
                return Ok(new { message = "Account Updated!!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                var userResponse = await _mediator.Send(new GetUserByIdRequest(tokenUserId));
                await _mediator.Send(new ChangePasswordRequest(userResponse.user, changePasswordDTO));
                return Ok(new { message = "Password changed!!" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [HttpPost("SendingEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> SendingEmail([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email content must not empty!" });

            var userResponse = await _mediator.Send(new GetUserByEmailRequest(email));

            var emailConfUrl = await _tokenServices.CreateTokenEmailConfirm(userResponse.user);
            var callback_url = _callBackURL.ToString() + userResponse.user?.Id + "&emailConfUrl=" + emailConfUrl;

            await _mailServices.SendingEmail(email, callback_url);
            return Ok(new { message = "Email verification code sended!!!" });
        }

        [HttpGet("EmailVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailVerification([FromQuery] string userId, [FromQuery] string emailConfUrl)
        {
            await _mailServices.ConfirmEmail(userId, emailConfUrl);
            return Ok(new { message = "Your email has been successfully confirmed!" });
        }


        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser([FromBody] RoleManagerDTO roleDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                await _mediator.Send(new AddRoleToUserRequest(roleDTO));
                return Ok(new { message = "Role added to user" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("RemoveRolFromUser")]
        public async Task<IActionResult> RemoveRolFromUser([FromBody] RoleManagerDTO roleDTO)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                await _mediator.Send(new RemoveRoleFromUserRequest(roleDTO));
                return Ok(new { message = "Role removed from user" });
            }
            return Unauthorized("Unvalid Token!!");
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("SuspendUser")]
        public async Task<IActionResult> SuspendUser([FromQuery] string userId)
        {
            var tokenUserId = User.FindFirst("uid")?.Value;
            if (!string.IsNullOrEmpty(tokenUserId))
            {
                await _mediator.Send(new SuspendUserRequest(userId));
                return Ok(new { message = "SuspendUser action done!" });
            }
            return Unauthorized("Unvalid Token!!");
        }
    }
}
