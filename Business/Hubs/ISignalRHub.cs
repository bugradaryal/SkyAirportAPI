using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Moderation;

namespace Business.Hubs
{
    public interface ISignalRHub
    {
        Task AddTicket(SupportTicket ticket);
        Task<List<SupportTicket>> GetTicketsByUser(string userId);
        Task<List<SupportTicket>> GetTicketsForSupport();
    }
}
