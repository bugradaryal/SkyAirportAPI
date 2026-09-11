using Entities;

namespace DataAccess.Abstract
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetAllByFlightId(int id);
        Task<Aircraft> GetAircraftByOwnedTicketId(int id);
        Task<bool> IsSeatAvailable(int id);
        Task SetSeatAvailable(int id, bool value);
    }
}
