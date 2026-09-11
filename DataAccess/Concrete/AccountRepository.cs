using DataAccess.Abstract;

namespace DataAccess.Concrete
{
    public class AccountRepository : IAccountRepository
    {
        private DataDbContext _dbContext;
        public AccountRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> SuspendUser(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsSuspended = !user.IsSuspended;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
