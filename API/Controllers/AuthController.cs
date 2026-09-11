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
using System.Net;
using Utilitys.Logging.ExceptionHandler;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ITokenServices _tokenServices;
        public AuthController(ITokenServices tokenServices) 
        {
            _tokenServices = tokenServices;
        }


        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "RefreshToken")] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new CustomException("Refresh token is required!", (int)HttpStatusCode.BadRequest);

            var user = await _tokenServices.ValidateRefleshToken(refreshToken);

            var newAccessToken = await _tokenServices.CreateTokenJWT(user);
            var newRefreshToken = _tokenServices.GenerateRefreshToken();
            await _tokenServices.SaveRefreshTokenAsync(user, newRefreshToken);

            return Ok(new { JwtToken = newAccessToken, RefreshToken = newRefreshToken });
        }
    }
}
