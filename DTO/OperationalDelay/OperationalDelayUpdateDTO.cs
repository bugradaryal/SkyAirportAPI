using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.OperationalDelay
{
    public class OperationalDelayUpdateDTO
    {
        public int id { get; set; }
        public string Delay_Reason { get; set; }
        public string Delay_Duration { get; set; }
        public DateTimeOffset Delay_Time { get; set; }
        public int flight_id { get; set; }
    }
}
