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
using Utilitys.ExceptionHandler;
using Utilitys.Logger;
using Utilitys.MailServices;
using Utilitys.Mapper;
using Business.Features.Account.Commands.SuspendUser;

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
        private readonly string _callBackURL;
        public AuthController(UserManager<User> userManager, IMapper mapper, IOptions<JwtBearer> jwt, IMediator mediator, IOptions<EmailSender> mail, ILoggerServices logger, IOptions<CallBackURL> callBackURL) 
        {
            _tokenServices = new TokenManager(jwt,userManager);
            _mapper = mapper;
            _mediator = mediator;
            _mailServices = new MailManager(mail,userManager);
            _phoneServices = new PhoneManager();
            _logger = logger;
            _callBackURL = callBackURL.Value.URL;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDTO createAccountDTO)
        {
            await _logger.Logger(new LogDTO{
                Message = "Register endpoint called for {"+createAccountDTO.UserName+"}",
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
            await _logger.Logger(new LogDTO
            {
                Message = "Login endpoint called for {"+loginAccountDTO.Email+"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                var userResponse = await _mediator.Send(new LoginRequest(loginAccountDTO));
                if (userResponse.error == false)
                {
                    if (userResponse.user.IsSuspended)
                    {
                        await _logger.Logger(new LogDTO
                        {
                            Message = "User is suspended!!",
                            Action_type = Action_Type.APIResponse,
                            Target_table = "User",
                            loglevel_id = 3,
                            user_id = userResponse.user?.Id
                        }, null);
                        return BadRequest(new { message = "User is suspended!!" });
                    }
                    var token = await _tokenServices.CreateTokenJWT(userResponse.user);
                    var roleResponse = await _mediator.Send(new GetUserRoleRequest(userResponse.user?.Id));
                    if (roleResponse.error == false)
                    {
                        var refleshtoken = _tokenServices.GenerateRefreshToken();
                        await _tokenServices.SaveRefreshTokenAsync(userResponse.user, refleshtoken);
                        await _logger.Logger(new LogDTO
                        {
                            Message = "Login Succeeded!",
                            Action_type = Action_Type.Login,
                            Target_table = "User",
                            loglevel_id = 1,
                            user_id = userResponse.user?.Id
                        }, null);
                        return Ok(new AuthenticationModel
                        {
                            Email = userResponse.user.Email,
                            Roles = roleResponse.UserRoles,
                            UserName = userResponse.user.UserName,
                            JwtToken = token,
                            RefreshToken = refleshtoken
                        });
                    }
                }
                await _logger.Logger(new LogDTO
                {
                    Message = userResponse.response?.Message + " for "+loginAccountDTO.Email,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = userResponse.response?.Exception?.ExceptionLevel,
                    user_id = userResponse.user?.Id
                }, userResponse.response?.Exception);
                return BadRequest(userResponse.response);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Login Succeeded!",
                Action_type = Action_Type.Login,
                Target_table = "User",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(_mapper.Map<AuthenticationModel,ValidateTokenDTO>(validateTokenDTO));         
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpDelete("DeleteAccount/{userId}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string userId)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeleteAccount endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
                user_id = userId
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid || userId != validateTokenDTO.user.Id)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = userId
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            else
            {
                var deleteResponse = await _mediator.Send(new DeleteAccountRequest(validateTokenDTO.user));
                if (deleteResponse != null)
                {
                    await _logger.Logger(new LogDTO
                    {
                        Message = deleteResponse.Message,
                        Action_type = Action_Type.APIResponse,
                        Target_table = "User",
                        loglevel_id = deleteResponse.Exception?.ExceptionLevel,
                        user_id = userId
                    }, deleteResponse.Exception);
                    return BadRequest(deleteResponse);
                }
                await _logger.Logger(new LogDTO
                {
                    Message = "Delete Succeeded!",
                    Action_type = Action_Type.Delete,
                    Target_table = "User",
                    loglevel_id = 1,
                    user_id = userId
                }, null);
                return Ok( new { message = "Account Deleted!!" });
            }

        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdateAccount endpoint called for {" + updateAccountDTO.UserName + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            else
            {
                User user = _mapper.Map<User,UpdateAccountDTO>(updateAccountDTO,validateTokenDTO.user);
                var updateResponse = await _mediator.Send(new UpdateAccountRequest(user));
                if(updateResponse != null)
                {
                    await _logger.Logger(new LogDTO
                    {
                        Message = updateResponse.Message,
                        Action_type = Action_Type.APIResponse,
                        Target_table = "User",
                        loglevel_id = updateResponse.Exception?.ExceptionLevel,
                        user_id=user.Id
                    }, updateResponse.Exception);
                    return BadRequest(updateResponse);
                }
                await _logger.Logger(new LogDTO
                {
                    Message = "Update Succeeded!",
                    Action_type = Action_Type.Update,
                    Target_table = "User",
                    loglevel_id = 1,
                    user_id = user.Id
                }, null);
                return Ok(new { message = "Account Updated!!" });
            }
        }

        [Authorize(Policy = "IsUserSuspended")]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "ChangePassword endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            else
            {
                var changePasswordResponse = await _mediator.Send(new ChangePasswordRequest(validateTokenDTO.user, changePasswordDTO));
                if(changePasswordResponse != null)
                {
                    await _logger.Logger(new LogDTO
                    {
                        Message = changePasswordResponse.Message,
                        Action_type = Action_Type.APIResponse,
                        Target_table = "User",
                        loglevel_id = changePasswordResponse.Exception.ExceptionLevel,
                        user_id = validateTokenDTO.user.Id,
                    }, changePasswordResponse.Exception);
                    return BadRequest(changePasswordResponse);
                }
                await _logger.Logger(new LogDTO
                {
                    Message = "Changing Password Succeeded!",
                    Action_type = Action_Type.Update,
                    Target_table = "User",
                    loglevel_id = 1,
                    user_id = validateTokenDTO.user.Id
                }, null);
                return Ok(new { message = "Password changed!!" });
            }
        }

        [HttpPost("SendingEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> SendingEmail([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "SendingEmail endpoint called but it was canceled because the email content was empty.",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3
                }, null);
                return BadRequest(new { message = "Email content must not empty!" });
            }
            await _logger.Logger(new LogDTO
            {
                Message = "SendingEmail endpoint called for {" + email.ToString() + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1
            }, null);

            var userResponse = await _mediator.Send(new GetUserByEmailRequest(email));
            if (userResponse.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = userResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = userResponse.response?.Exception?.ExceptionLevel,
                    user_id = userResponse.user?.Id ?? null
                }, userResponse.response?.Exception);
                return BadRequest(userResponse.response);
            }
            var emailConfUrl = await _tokenServices.CreateTokenEmailConfirm(userResponse.user);
            var callback_url = _callBackURL.ToString() + userResponse.user?.Id + "&emailConfUrl=" + emailConfUrl;

            var mailResponse = await _mailServices.SendingEmail(email, callback_url);
            if(mailResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = mailResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = mailResponse.Exception.ExceptionLevel,
                    user_id = userResponse.user?.Id
                }, mailResponse.Exception);
                return BadRequest(mailResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Email verification code sended!!!",
                Action_type = Action_Type.APIResponse,
                Target_table = "User",
                loglevel_id = 1,
                user_id = userResponse.user?.Id
            }, null);
            return Ok(new { message = "Email verification code sended!!!" });
        }
        [HttpGet("EmailVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailVerification([FromQuery] string userId, [FromQuery] string emailConfUrl)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "EmailVerification endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
                user_id = userId,
            }, null);
            var mailResponse = await _mailServices.ConfirmEmail(userId, emailConfUrl);
            if (mailResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = mailResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = mailResponse.Exception.ExceptionLevel,
                    user_id = userId
                }, mailResponse.Exception);
                return BadRequest(mailResponse);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Email Verificated!",
                Action_type = Action_Type.APIResponse,
                Target_table = "User",
                loglevel_id = 1,
                user_id = userId,
            }, null);
            return Ok(new { message = "Your email has been successfully confirmed!" });
        }

        [HttpPost("ValidateToken")]
        [Authorize(Policy = "IsUserSuspended")]
        public async Task<IActionResult> ValidateToken()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "ValidateToken endpoint called.",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
            }, null);
            var result = await _tokenServices.ValidateToken(this.HttpContext);
            if (result.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = result.user?.Id
                }, null);
                if (result.user.IsSuspended)
                {
                    await _logger.Logger(new LogDTO
                    {
                        Message = "User is suspended!",
                        Action_type = Action_Type.APIResponse,
                        Target_table = "User",
                        loglevel_id = 3,
                        user_id = result.user.Id
                    },null);
                    return BadRequest(new { message = "User is suspended!!" });
                }
                await _logger.Logger(new LogDTO
                {
                    Message = "Token valitaded!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 1,
                    user_id= result.user.Id
                }, null);
                return Ok(result);
            }
            await _logger.Logger(new LogDTO
            {
                Message = "Token is not valid.",
                Action_type = Action_Type.APIResponse,
                Target_table = "User",
                loglevel_id = 3,
                user_id = result.user?.Id ?? null
            }, null);
            return BadRequest(new { message = "Token is not valid!!!" });
        }


        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser([FromBody] RoleManagerDTO roleDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddRoleToUser endpoint called for {" + roleDTO.userId ?? null +"}",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            var roleResponse = await _mediator.Send(new AddRoleToUserRequest(roleDTO));
            if (roleResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = roleResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = roleResponse.Exception?.ExceptionLevel,
                    user_id = validateTokenDTO.user?.Id
                }, roleResponse.Exception);
                return BadRequest(roleResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Role added to user",
                Action_type = Action_Type.APIResponse,
                Target_table = "User",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Role added to user" });
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("RemoveRolFromUser")]
        public async Task<IActionResult> RemoveRolFromUser([FromBody] RoleManagerDTO roleDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "RemoveRolFromUser endpoint called for {" + roleDTO.userId ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            var roleResponse = await _mediator.Send(new RemoveRoleFromUserRequest(roleDTO));
            if (roleResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = roleResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = roleResponse.Exception?.ExceptionLevel,
                    user_id = validateTokenDTO.user?.Id
                }, roleResponse.Exception);
                return BadRequest(roleResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Role removed from user",
                Action_type = Action_Type.APIResponse,
                Target_table = "User",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Role removed from user" });
        }

        [Authorize(Roles = "Administrator", Policy = "IsUserSuspended")]
        [HttpPost("SuspendUser")]
        public async Task<IActionResult> SuspendUser([FromQuery] string userId)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "SuspendUser endpoint called for {" + userId ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "User",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
                return Unauthorized(new { message = "Token not valid!!" });
            }
            var suspenResponse = await _mediator.Send(new SuspendUserRequest(userId));
            if (suspenResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = suspenResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = suspenResponse.Exception?.ExceptionLevel,
                    user_id = validateTokenDTO.user?.Id
                }, suspenResponse.Exception);
                return BadRequest(suspenResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "SuspendUser action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "User",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "SuspendUser action done!" });
        }
    }
}
