
namespace DataAccess.Abstract
{
    public interface IAircraftStatusRepository
    {
        Task<bool> AnyStatus(string status);
    }
}
