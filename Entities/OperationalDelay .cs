using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OperationalDelay
    {
        public int id { get; set; }
        public string Delay_Reason { get; set; }
        public string Delay_Duration { get; set; }
        public DateTime Delay_Time { get; set; }


        public int flight_id { get; set; }
        public Flight flight { get; set; }
    }
}
