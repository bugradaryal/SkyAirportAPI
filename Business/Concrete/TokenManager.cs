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
using Utilitys.ExceptionHandler;
using System.Net;
using DTO.Account;

namespace Business.Concrete
{
    public class TokenManager : ITokenServices
    {
        private readonly JwtBearer _jwt;
        public UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _key;
        private readonly ITokenRepository _tokenRepository;
        public TokenManager(IOptions<JwtBearer> jwt, UserManager<User> userManager)
        {
            _jwt = jwt.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            _userManager = userManager;
            _tokenRepository = new TokenRepository();
        }


        public async Task<ValidateTokenDTO> ValidateToken(HttpContext httpContext)
        {
            try
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

                if (!string.IsNullOrEmpty(token))
                {
                    if(token.StartsWith("Bearer "))
                    {
                        var clearToken = token.Substring("Bearer ".Length).Trim();
                        var principal = tokenHandler.ValidateToken(clearToken, validationParameters, out var validatedToken);
                        if((validatedToken is JwtSecurityToken jwtToken))
                        {
                            var userId = principal.FindFirst("uid")?.Value;
                            var claimUser = await _userManager.FindByIdAsync(userId);
                            if (claimUser != null)
                            {
                                var userRole = await _userManager.GetRolesAsync(claimUser);
                                return new ValidateTokenDTO { user = claimUser, roles = userRole.ToList(), IsTokenValid = true };
                            }
                        }
                    }
                }
                var refreshToken = httpContext.Request.Headers["RefreshToken"].FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    var userToken = await _tokenRepository.GetUserTokenByRefreshTokenAsync(refreshToken);
                    if (userToken != null)
                    {
                        var claimuser2 = await _userManager.FindByIdAsync(userToken.UserId);
                        if (claimuser2 != null)
                        {
                            var userRole2 = await _userManager.GetRolesAsync(claimuser2);
                            return new ValidateTokenDTO { user = claimuser2, roles = userRole2.ToList(), IsTokenValid = true };
                        }
                    }
                    throw new CustomException("Reflesh token corrupted!!",3,(int)HttpStatusCode.BadRequest);
                }
                return new ValidateTokenDTO { user = null, roles = null, IsTokenValid = false };
            }
            catch (Exception ex) 
            {
                throw new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }

        }

        public async Task<string> CreateTokenJWT(User user)
        {
            try
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
            catch(Exception ex) 
            {
                throw new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }

        }

        public async Task<string> CreateTokenEmailConfirm(User user)
        {
            try
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string encodedToken = HttpUtility.UrlEncode(token);
                return encodedToken;
            }
            catch (Exception ex) 
            {
                throw new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }
        }

        public string GenerateRefreshToken()
        {
            try
            {
                var randomBytes = new byte[32]; // 32 byte uzunluğunda bir byte dizisi
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(randomBytes); // Rastgele byte dizisi oluşturuluyor
                }
                return Convert.ToBase64String(randomBytes); // Base64 formatında döndürülür
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }
        }
        public async Task SaveRefreshTokenAsync(User user, string refreshToken)
        {
            try
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
                var result = await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", refreshToken);
                if (!result.Succeeded)
                {
                    throw new CustomException("Refresh token save failed.", 4, (int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }
        }
    }
}