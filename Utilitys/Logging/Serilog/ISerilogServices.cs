using Entities.Moderation;

namespace Utilitys.Logging.Serilog
{
    public interface ISerilogServices
    {
        void Info(LogEntry logDTO);
        void Warn(LogEntry logDTO);
        void Error(LogEntry logDTO, Exception ex = null);
        public void Fatal(LogEntry logDTO, Exception ex = null);
    }
}
