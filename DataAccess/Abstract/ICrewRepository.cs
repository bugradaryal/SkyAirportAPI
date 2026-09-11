using Entities;

namespace DataAccess.Abstract
{
    public interface ICrewRepository
    {
        Task<List<Crew>> GetAllByAircraftId(int id);
    }
}
