using DataAccess.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class SeatRepository : ISeatRepository
    {
        private DataDbContext _dbContext;
        public SeatRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Seat>> GetAllByFlightId(int id)
        {
            return await _dbContext.Seats.Where(x => x.flight_id == id).ToListAsync();
        }
        public async Task<Aircraft> GetAircraftByOwnedTicketId(int id)
        {
            var result = await (
                from ownedTicket in _dbContext.OwnedTickets
                join ticket in _dbContext.Tickets on ownedTicket.ticket_id equals ticket.id
                join seat in _dbContext.Seats on ticket.seat_id equals seat.id
                join flightAircraft in _dbContext.Flight_Aircrafts on seat.flight_id equals flightAircraft.flight_id
                join aircraft in _dbContext.Aircrafts on flightAircraft.aircraft_id equals aircraft.id
                where ownedTicket.id == id
                select aircraft
            ).FirstOrDefaultAsync();

            return result;
        }

        public async Task SetSeatAvailable(int id, bool value)
        {
            await _dbContext.Seats
                .Where(s => _dbContext.Tickets
                    .Where(t => t.id == id)
                    .Select(t => t.seat_id)
                    .Contains(s.id))
                .ExecuteUpdateAsync(seat => seat
                    .SetProperty(s => s.Is_Available, value));
        }

        public async Task<bool> IsSeatAvailable(int id)
        {
            return await _dbContext.Seats.Where(x => x.id == id).AnyAsync(z => z.Is_Available == true);
        }
    }
}
