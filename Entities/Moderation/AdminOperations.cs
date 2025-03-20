using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Moderation
{
    public class AdminOperations
    {
        public int id { get; set; }
        public string Operation_type { get; set; }
        public string Target_table { get; set; }
        public int Target_id { get; set; }
        public DateTime Operation_Date { get; set; }


        public int user_id { get; set; }
        public User user { get; set; }
    }
}
