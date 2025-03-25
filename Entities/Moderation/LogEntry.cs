using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Entities.Moderation
{
    public class LogEntry
    {
        public int id { get; set; }
        public DateTime Timestamp { get; set; }
        //public string Log_level { get; set; }
        public string Message { get; set; }
        public Action_Type Action_type { get; set; }
        public string Target_table { get; set; }
        public int Record_id { get; set; }
        public List<string> AdditionalData { get; set; } //JSON


        public string user_id { get; set; }
        public int loglevel_id { get; set; }
        public User user { get; set; }
        public LogLevel logLevel { get; set; }
    }
}
