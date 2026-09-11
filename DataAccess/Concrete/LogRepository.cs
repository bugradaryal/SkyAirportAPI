using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using Entities;
using Entities.Moderation;
using Microsoft.EntityFrameworkCore;

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
