using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Moderation;

namespace DataAccess.Abstract
{
    public interface ISupportTicketRepository
    {
        Task AddTicket(SupportTicket supportTicket);
        Task<List<SupportTicket>> GetTicketsByUser(string userId);
        Task<List<SupportTicket>> GetTicketsForSupport();
        Task<SupportTicket> GetTicketById(int ticketId);
        Task UpdateTicket(SupportTicket supportTicket);
    }
}
