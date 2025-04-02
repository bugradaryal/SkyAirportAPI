using DTO;
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

namespace Business.Concrete
{
    public class TokenManager : ITokenServices
    {
        private readonly JWT_Conf _jwt;
        public UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _key;
        public TokenManager(IOptions<JWT_Conf> jwt, UserManager<User> userManager)
        {
            _jwt = jwt.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            _userManager = userManager;
        }


        public async Task<ValidateTokenDTO> ValidateToken(HttpContext httpContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
                ValidIssuer = _jwt.Issuer,
                ValidAudience = _jwt.Audience,
            };
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(token) || token.StartsWith("Bearer "))
            {
                var clearToken = token.Substring("Bearer ".Length).Trim();
                var principal = tokenHandler.ValidateToken(clearToken, validationParameters, out var validatedToken);
                if((validatedToken is JwtSecurityToken jwtToken))
                {
                    var userId = principal.FindFirst("uid")?.Value;
                    var claimUser = await _userManager.FindByIdAsync(userId);
                    if (claimUser == null)
                        return new ValidateTokenDTO { user = null, roles = null, IsTokenValid = false, Token = null };

                    var userRole = await _userManager.GetRolesAsync(claimUser);
                    return new ValidateTokenDTO { user = claimUser, roles = userRole.ToList(), IsTokenValid = true, Token = token };
                }
            }
            return new ValidateTokenDTO { user = null, roles = null, IsTokenValid = false, Token = null };
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
                new Claim("IsSuspended", user.IsSuspended.ToString()) // 'added' claim'i ekleniyor
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

            return tokenHandler.WriteToken(token);
        }
    }
}