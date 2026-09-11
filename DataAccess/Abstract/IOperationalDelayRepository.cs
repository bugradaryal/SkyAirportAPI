using Entities;

namespace DataAccess.Abstract
{
    public interface IOperationalDelayRepository
    {
        Task<List<OperationalDelay>> GetAllByFlightId(int id);
    }
}
