using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Moderation
{
    public class LogLevel
    {
        public int id { get; set; }
        public string Level { get; set; }


        public ICollection<LogEntry> logEntry { get; set; }
    }
}
