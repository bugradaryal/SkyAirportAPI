using DataAccess.Abstract;
using DataAccess.Concrete;
using DTO.Account;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Bussines.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly ISupportTicketRepository _hubServices;

        public SignalRHub()
        {
            _hubServices = new SupportTicketRepository();
        }
        // Bir destek talebi mesajını destek temsilcisine gönderir
        public async Task SendMessageToSupport(string userId, string userName, string message, int ticketId)
        {
            var ticket = await _hubServices.GetTicketById(ticketId);
            if (ticket != null)
            {
                // Mesajı temsilciye gönder
                await Clients.Group(userId).SendAsync("ReceiveMessage", message);
                ticket.Status = SupportTicket_Status.New.ToString();
                await _hubServices.AddTicket(ticket);
            }
        }

        // Destek temsilcisinin gruba katılmasını sağlar
        public async Task JoinAsSupportAgent(string agentUserId, int ticketId)
        {
            var ticket = await _hubServices.GetTicketById(ticketId);
            if (ticket != null)
            {
                // Mesajı temsilciye gönder
                await Groups.AddToGroupAsync(Context.ConnectionId, agentUserId);
                ticket.Status = SupportTicket_Status.Viewed.ToString();
                await _hubServices.UpdateTicket(ticket);
            }
        }

        // Destek talebi tamamlandığında grubun bilgilendirilmesi
        public async Task EndSupportSession(string agentUserName, int ticketId)
        {
            var ticket = await _hubServices.GetTicketById(ticketId);
            if (ticket != null)
            {
                // Mesajı temsilciye gönder
                await Clients.Group(agentUserName).SendAsync("SupportSessionEnded", "The ticket has been resolved.");
                ticket.Status = SupportTicket_Status.Closed.ToString();
                await _hubServices.UpdateTicket(ticket);
            }
        }
    }
}