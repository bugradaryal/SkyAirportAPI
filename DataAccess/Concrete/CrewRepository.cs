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
    public class CrewRepository : ICrewRepository
    {
        private DataDbContext _dbContext;
        public CrewRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Crew>> GetAllByAircraftId(int id)
        {

            var crew = await _dbContext.Crew_Aircrafts
                .Where(af => af.aircraft_id == id)
                .Include(af => af.crew)
                .Select(af => af.crew)
                .ToListAsync();
            return crew;
        }
    }
}
