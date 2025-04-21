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
    public class FlightRepository : IFlightRepository
    {
        public async Task<List<Flight>> GetAllByAirlineId(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Flights.Where(x => x.airline_id == id).ToListAsync();
            }
        }
    }
}
