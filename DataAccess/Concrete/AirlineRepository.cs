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
    public class AirlineRepository : IAirlineRepository
    {
        private DataDbContext _dbContext;
        public AirlineRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Airline>> GetAllByAirportId(int id)
        {
            return await _dbContext.Airlines.Where(x => x.airport_id == id).ToListAsync();
        }
    }
}
