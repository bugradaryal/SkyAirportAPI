using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using Entities;
using Entities.Enums;
using Entities.Moderation;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        // Yeni bir ticket ekle
        public async Task AddTicket(SupportTicket supportTicket)
        {
            using (var _dbContext = new DataDbContext())
            {
                _dbContext.SupportTickets.Add(supportTicket);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Ticket'ları kullanıcıya göre al
        public async Task<List<SupportTicket>> GetTicketsByUser(string userId)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.SupportTickets
                .Where(t => t.UserId == userId)
                .ToListAsync();
            }
        }

        // Destek temsilcilerine atanmış ticket'ları al
        public async Task<List<SupportTicket>> GetTicketsForSupport()
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.SupportTickets
                .Where(t => t.Status == SupportTicket_Status.New.ToString())
                .ToListAsync();
            }
        }

        public async Task<SupportTicket> GetTicketById(int ticketId)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.SupportTickets.Where(x => x.id == ticketId).FirstOrDefaultAsync();
            }
        }

        public async Task UpdateTicket(SupportTicket supportTicket)
        {
            using (var _dbContext = new DataDbContext())
            {
                _dbContext.SupportTickets.Update(supportTicket);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}