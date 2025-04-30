using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Utilitys.Logger
{
    public class SerilogLogger : ILoggerServices
    {
        public void Info(string message)
        {
            Log.Information(message);
        }

        public void Warn(string message)
        {
            Log.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            Log.Error(ex, message);
        }
    }
}
