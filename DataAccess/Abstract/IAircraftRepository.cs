using Entities;

namespace DataAccess.Abstract
{
    public interface IAircraftRepository
    {
        Task<List<Aircraft>> GetAll();
        Task<List<Aircraft>> GetAllById(int id);
    }
}
