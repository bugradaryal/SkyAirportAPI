using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class AccountRepository : IAccountRepository
    {
        public async Task SuspendUser(string userId)
        {
            using (var _dbContext = new DataDbContext())
            {
                bool suspend = false;
                var user = new User { Id = userId };
                _dbContext.Users.Attach(user);

                if (!user.IsSuspended)
                    suspend = true;

                    _dbContext.Entry(user).Property(u => u.IsSuspended).IsModified = true;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
