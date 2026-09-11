using Entities;

namespace DataAccess.Abstract
{
    public interface IPersonalRepository
    {
        Task<List<Personal>> GetAllByAirportId(int id);
    }
}
