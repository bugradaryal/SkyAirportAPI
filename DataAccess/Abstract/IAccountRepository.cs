namespace DataAccess.Abstract
{
    public interface IAccountRepository
    {
        Task<bool> SuspendUser(string userId);
    }
}
