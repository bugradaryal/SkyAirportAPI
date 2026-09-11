using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Entities.Moderation;
using Serilog;

namespace Utilitys.Logging.Serilog
{
    public class SerilogLogger : ISerilogServices
    {
        public void Info(LogEntry logDTO)
        {
            Log.Information("{@Log}", logDTO);
        }

        public void Warn(LogEntry logDTO)
        {
            Log.Warning("{@Log}", logDTO);
        }

        public void Error(LogEntry logDTO, Exception ex = null)
        {
            Log.Error(ex, "{@Log}", logDTO);
        }

        public void Fatal(LogEntry logDTO, Exception ex = null)
        {
            Log.Fatal(ex, "{@Log}", logDTO);
        }
    }
}
