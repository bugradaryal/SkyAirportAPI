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
    public class OperationalDelayRepository : IOperationalDelayRepository
    {
        private DataDbContext _dbContext;
        public OperationalDelayRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<OperationalDelay>> GetAllByFlightId(int id)
        {

            return await _dbContext.OperationalDelays.Where(x => x.flight_id == id).ToListAsync();
        }
    }
}
