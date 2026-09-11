using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class AircraftStatusRepository : IAircraftStatusRepository
    {
        private DataDbContext _dbContext;
        public AircraftStatusRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AnyStatus(string status)
        {
            return await _dbContext.AircraftStatuses.AnyAsync(x => x.Status == status);
        }
    }
}
