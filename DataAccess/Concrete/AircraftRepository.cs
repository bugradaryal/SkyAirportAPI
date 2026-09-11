using DataAccess.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class AircraftRepository : IAircraftRepository
    {
        private DataDbContext _dbContext;
        public AircraftRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Aircraft>> GetAllById(int id)
        {
            return await _dbContext.Aircrafts.Where(x => x.id == id).Include(g => g.aircraftStatus).ToListAsync();
        }
        public async Task<List<Aircraft>> GetAll()
        {
            return await _dbContext.Aircrafts.Include(g => g.aircraftStatus).ToListAsync();
        }
    }
}
