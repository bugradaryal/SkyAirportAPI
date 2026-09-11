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
    public class OwnedTicketRepository : IOwnedTicketRepository
    {
        private DataDbContext _dbContext;
        public OwnedTicketRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<decimal> GetTicketWeightById(int id)
        {

            return await _dbContext.OwnedTickets.Where(t => t.id == id).Select(t => t.Baggage_weight).FirstOrDefaultAsync();
        }
        public async Task<List<OwnedTicket>> GetTicketBySeatId(int id)
        {

            return await (from ownedTicket in _dbContext.OwnedTickets
            join ticket in _dbContext.Tickets on ownedTicket.ticket_id equals ticket.id
            join seat in _dbContext.Seats on ticket.seat_id equals seat.id 
            where seat.id == id
            select ownedTicket).ToListAsync();
        }
    }
}
