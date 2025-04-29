using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Flight
{
    public class FlightAddDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Arrival_Date { get; set; }
        public DateTimeOffset Deperture_Date { get; set; }
        public string Status { get; set; }
        public int airline_id { get; set; }
        public int aircraft_id { get; set; }
    }
}
