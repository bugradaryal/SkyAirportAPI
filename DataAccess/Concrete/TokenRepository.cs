using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class TokenRepository : ITokenRepository
    {
        private DataDbContext _dbContext;
        public TokenRepository(DataDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<IdentityUserToken<string>> GetUserTokenByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Set<IdentityUserToken<string>>()
                .FirstOrDefaultAsync(t => t.Name == "RefreshToken" && t.Value == refreshToken);
        }
    }
}
