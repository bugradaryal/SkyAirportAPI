using Entities.Moderation;

namespace DataAccess.Abstract
{
    public interface ILogRepository
    {
        Task AddLog(LogEntry log);
    }
}
