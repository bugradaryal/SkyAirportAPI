using Entities.Configuration;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Business.Abstract;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Security.Cryptography;
using DataAccess.Abstract;
using DataAccess.Concrete;
using System.Net;
using DTO.Account;
using Utilitys.Logging.ExceptionHandler;

namespace Business.Concrete
{
    public class TokenManager : ITokenServices
    {
        private readonly JwtBearer _jwt;
        public UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _key;
        private readonly ITokenRepository _tokenRepository;
        public TokenManager(IOptions<JwtBearer> jwt, UserManager<User> userManager, ITokenRepository tokenRepository)
        {
            _jwt = jwt.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        public async Task<string> CreateTokenJWT(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in userRoles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var claims = new List<Claim>
                {
                    new Claim("uid", user.Id),
                    new Claim("IsSuspended", user.IsSuspended.ToString().ToLower())
                };
            claims.AddRange(roleClaims);
            var signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescrtiptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                SigningCredentials = signingCredentials,
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescrtiptor);

            return "Bearer " + tokenHandler.WriteToken(token);
        }

        public async Task<string> CreateTokenEmailConfirm(User user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string encodedToken = HttpUtility.UrlEncode(token);
            return encodedToken;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(32);  
            return Convert.ToBase64String(randomBytes);
        }

        public async Task SaveRefreshTokenAsync(User user, string refreshToken)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
            var result = await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", refreshToken);
            if (!result.Succeeded)
                throw new CustomException("Refresh token save failed.", (int)HttpStatusCode.InternalServerError, result.Errors?.FirstOrDefault()?.ToString());
        }

        public async Task<User> ValidateRefleshToken(string refreshToken)
        {
            var userToken = await _tokenRepository.GetUserTokenByRefreshTokenAsync(refreshToken);
            if(userToken != null)
            {
                var user = await _userManager.FindByIdAsync(userToken.UserId);
                return user;
            }
            throw new CustomException("Reflesh Token corrupted",(int)HttpStatusCode.BadRequest);
        }
    }
}