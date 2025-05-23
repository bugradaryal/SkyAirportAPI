﻿using DTO.Account;
using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITokenServices
    {
        Task<ValidateTokenDTO> ValidateToken(HttpContext httpContext);
        Task<string> CreateTokenJWT(User user);
        Task<string> CreateTokenEmailConfirm(User user);
        Task SaveRefreshTokenAsync(User user, string refreshToken);
        string GenerateRefreshToken();
    }
}
