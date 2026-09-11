using Entities;

namespace DataAccess.Abstract
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetAllByAirlineId(int id);
        Task<List<Flight>> GetAllByAircraftId(int id);
    }
}
