using Business.Abstract;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Utilitys.Logging;
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


        [LogAction(Action_Type.Update)]
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