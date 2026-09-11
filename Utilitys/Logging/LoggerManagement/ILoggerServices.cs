using DTO;
using Utilitys.Logging.ExceptionHandler;

namespace Utilitys.Logging
{
    public interface ILoggerServices
    {
        Task Logger(LogDTO logdto, CustomException? exception = null);
    }
}
