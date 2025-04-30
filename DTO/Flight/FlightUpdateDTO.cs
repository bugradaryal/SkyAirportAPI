using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Flight
{
    public class FlightUpdateDTO
    {
        [Required]
        public int id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public DateTimeOffset Arrival_Date { get; set; }

        public DateTimeOffset Deperture_Date { get; set; }

        [StringLength(38)]
        public string Status { get; set; }
        public int airline_id { get; set; }
        public int aircraft_id { get; set; }
    }
}
