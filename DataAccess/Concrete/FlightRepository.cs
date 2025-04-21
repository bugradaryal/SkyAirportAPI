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
        public async Task<List<Flight>> GetAllByAircraftId(int id)
        {
            using (var _dbContext = new DataDbContext())
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
}
