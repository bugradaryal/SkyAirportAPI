using Microsoft.AspNetCore.Identity;

namespace DataAccess.Abstract
{
    public interface ITokenRepository
    {
        Task<IdentityUserToken<string>> GetUserTokenByRefreshTokenAsync(string refreshToken);
    }
}
