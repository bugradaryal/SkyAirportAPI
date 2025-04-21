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
    public class AircraftRepository : IAircraftRepository
    {
        public async Task<List<Aircraft>> GetAllById(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Aircrafts.Where(x => x.id == id).Include(g => g.aircraftStatus).ToListAsync();
            }
        }
        public async Task<List<Aircraft>> GetAll()
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Aircrafts.Include(g => g.aircraftStatus).ToListAsync();
            }
        }
    }
}
