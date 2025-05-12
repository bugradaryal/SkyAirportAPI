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
        public async Task<List<OwnedTicket>> GetAllBySeatId(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.OwnedTickets.Where(x => x.seat_id == id).ToListAsync();
            }
        }

        public async Task<decimal> GetTicketWeightById(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.OwnedTickets.Where(t => t.id == id).Select(t => t.Baggage_weight).FirstOrDefaultAsync();
            }
        }
    }
}
