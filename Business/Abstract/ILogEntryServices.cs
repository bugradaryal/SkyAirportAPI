using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ILogEntryServices
    {
        void Logger(string userId, int LogLevel, string actionType, string message, string tableName = "", int recordId = 0, List<string> additionalData = null);
    }
}
