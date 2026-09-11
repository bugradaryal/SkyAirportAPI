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
        private DataDbContext _dbContext;
        public FlightRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Flight>> GetAllByAirlineId(int id)
        {
            return await _dbContext.Flights.Where(x => x.airline_id == id).ToListAsync();
        }
        public async Task<List<Flight>> GetAllByAircraftId(int id)
        {
            var flights = await _dbContext.Flight_Aircrafts
                .Where(af => af.aircraft_id == id)
                .Include(af => af.flight)
                .Select(af => af.flight)
                .ToListAsync();
            return flights;
        }
    }
}
