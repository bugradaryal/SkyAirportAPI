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
        public string Log_level { get; set; }
        public string Message { get; set; }
        public string Action_type { get; set; }
        public string Table_name { get; set; }
        public int Record_id { get; set; }
        public List<string> AdditionalData { get; set; } //JSON


        public int user_id { get; set; }
        public User user { get; set; }
    }
}
