using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.OperationalDelay
{
    public class OperationalDelayAddDTO
    {
        [Required]
        [MaxLength(1024)]
        public string Delay_Reason { get; set; }
        [Required]
        [MaxLength(12)]
        public string Delay_Duration { get; set; }
        [Required]
        public DateTimeOffset Delay_Time { get; set; }
        [Required]
        public int flight_id { get; set; }
    }
}
