using Business.Abstract;
using Business.Concrete;
using Business.Features.Account.Commands.ChangePassword;
using Business.Features.Account.Commands.CreateAccount;
using Business.Features.Account.Commands.DeleteAccount;
using Business.Features.Account.Commands.UpdateAccount;
using Business.Features.Account.Queries.GetUserByEmail;
using Business.Features.Account.Queries.GetUserRole;
using Business.Features.Account.Queries.Login;
using DTO;
using DTO.Account;
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
using Utilitys.ExceptionHandler;
using Utilitys.Logger;
using Utilitys.MailServices;
using Utilitys.Mapper;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        private readonly IMailServices _mailServices;
        private readonly IPhoneServices _phoneServices;
        public AuthController(UserManager<User> userManager, IMapper mapper, IOptions<JwtBearer> jwt, IMediator mediator, IOptions<EmailSender> mail, ILoggerServices logger) 
        {
            _tokenServices = new TokenManager(jwt,userManager);
            _mapper = mapper;
            _mediator = mediator;
            _mailServices = new MailManager(mail,userManager);
            _phoneServices = new PhoneManager();
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDTO createAccountDTO)
        {
            await _logger.Logger(new LogDTO{
                Message = "Register endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
            },null);

            var createResponse = await _mediator.Send(new CreateAccountRequest(createAccountDTO));
            if(createResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = createResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = createResponse.Exception.ExceptionLevel
                }, createResponse.Exception);
                return BadRequest(createResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Account Created!",
                Action_type = Action_Type.Create,
                Target_table = "User",
                loglevel_id = 1,
            }, null);
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
                        var refleshtoken = _tokenServices.GenerateRefreshToken();
                        await _tokenServices.SaveRefreshTokenAsync(userResponse.user, refleshtoken);
                        return Ok(new AuthenticationModel
                        {
                            Email = userResponse.user.Email,
                            Roles = roleResponse.UserRoles,
                            UserName = userResponse.user.UserName,
                            JwtToken = token,
                            RefreshToken = refleshtoken
                        });
                    }
                    return BadRequest(roleResponse.response.Exception);
                }
                return BadRequest(userResponse.response.Exception);
            }
            return Ok(_mapper.Map<AuthenticationModel,ValidateTokenDTO>(validateTokenDTO));         
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAccount/{userId}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string userId)
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
                User user = _mapper.Map<User,UpdateAccountDTO>(updateAccountDTO,validateTokenDTO.user);
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

        [HttpPost("SendingEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> SendingEmail([FromBody] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest(new { message = "Account does not exist!" });

                var mailResponse = await _mediator.Send(new GetUserByEmailRequest(email));
                if(mailResponse.error == true)
                    return BadRequest(mailResponse.response.Exception);
                var emailConfUrl = await _tokenServices.CreateTokenEmailConfirm(mailResponse.user);
                var callback_url = "https://localhost:7257/EmailVerification?userId=" + mailResponse.user.Id + "&emailConfUrl=" + emailConfUrl;


                await _mailServices.SendingEmail(email, callback_url);
                return Ok(new { message = "Email verification code sended!!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("EmailVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailVerification([FromQuery] string userId, [FromQuery] string emailConfUrl)
        {
            try
            {
                await _mailServices.ConfirmEmail(userId, emailConfUrl);
                return Ok(new { message = "Your email has been successfully confirmed!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("ValidateToken")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                var result = await _tokenServices.ValidateToken(this.HttpContext);
                if(result.IsTokenValid)
                {                
                    if (result.user.IsSuspended)
                        return BadRequest(new { message = "User is suspended!!" });
                    return Ok(result);
                }
                return BadRequest(new { message = "Token is not valid!!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
