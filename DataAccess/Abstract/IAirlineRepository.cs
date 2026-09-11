using Entities;

namespace DataAccess.Abstract
{
    public interface IAirlineRepository
    {
        Task<List<Airline>> GetAllByAirportId(int id);
    }
}
