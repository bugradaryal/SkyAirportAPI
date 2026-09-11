using DataAccess.Abstract;
using Entities.Moderation;

namespace DataAccess.Concrete
{
    public class LogRepository : ILogRepository
    {
        private DataDbContext _dbContext;
        public LogRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddLog(LogEntry log)
        {
            await _dbContext.LogEntrys.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
