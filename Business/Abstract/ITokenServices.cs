using Entities;

namespace Business.Abstract
{
    public interface ITokenServices
    {
        Task<string> CreateTokenJWT(User user);
        Task<string> CreateTokenEmailConfirm(User user);
        Task SaveRefreshTokenAsync(User user, string refreshToken);
        string GenerateRefreshToken();
        Task<User> ValidateRefleshToken(string refreshToken);
    }
}
