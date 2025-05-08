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
        public async Task AddLog(LogEntry log)
        {
            using (var _dbContext = new DataDbContext())
            {
                await _dbContext.LogEntrys.AddAsync(log);
                await _dbContext.SaveChangesAsync();    
            }
        }
    }
}
