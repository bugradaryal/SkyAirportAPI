using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Abstract
{
    public interface ITokenRepository
    {
        Task<IdentityUserToken<string>> GetUserTokenByRefreshTokenAsync(string refreshToken);
    }
}
