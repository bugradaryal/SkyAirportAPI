using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.OperationalDelay
{
    public class OperationalDelayUpdateDTO
    {
        [Required]
        public int id { get; set; }

        [MaxLength(1024)]
        public string Delay_Reason { get; set; }

        [MaxLength(12)]
        public string Delay_Duration { get; set; }
        public DateTimeOffset Delay_Time { get; set; }
        public int flight_id { get; set; }
    }
}
