using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Moderation
{
    public class AdminOperation
    {
        public int id { get; set; }
        public Operation_Type Operation_type { get; set; }
        public string Target_table { get; set; }
        public int Target_id { get; set; }
        public DateTime Operation_Date { get; set; }


        public string user_id { get; set; }
        public User user { get; set; }
    }
}
