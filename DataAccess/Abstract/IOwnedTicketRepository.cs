using Entities;

namespace DataAccess.Abstract
{
    public interface IOwnedTicketRepository
    {
        Task<decimal> GetTicketWeightById(int id);
        Task<List<OwnedTicket>> GetTicketBySeatId(int id);
    }
}
