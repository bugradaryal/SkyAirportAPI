using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class SeatRepository : ISeatRepository
    {
        public async Task<List<Seat>> GetAllByFlightId(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Seats.Where(x => x.flight_id == id).ToListAsync();
            }
        }
        public async Task<Aircraft> GetSeatAndAircraftByTicketId(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                var result = await (
                    from ticket in _dbContext.OwnedTickets
                    join seat in _dbContext.Seats on ticket.seat_id equals seat.id
                    join flightAircraft in _dbContext.Flight_Aircrafts on seat.flight_id equals flightAircraft.flight_id
                    join aircraft in _dbContext.Aircrafts on flightAircraft.aircraft_id equals aircraft.id
                    where ticket.id == id
                    select aircraft
                ).FirstOrDefaultAsync();
                return result;
            }
        }

        public async Task SetSeatAvailable(int id, bool value)
        {
            using (var _dbContext = new DataDbContext())
            {
                await _dbContext.Seats
                    .Where(s => s.id == id).ExecuteUpdateAsync(s => s.SetProperty(seat => seat.Is_Available, value));
            }
        }

        public async Task<bool> IsSeatAvailable(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Seats.Where(x => x.id == id).AnyAsync(z => z.Is_Available == true);
            }
        }
    }
}
