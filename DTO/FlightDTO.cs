using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class FlightDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Arrival_Date { get; set; }
        public DateTime Deperture_Date { get; set; }
        public string Status { get; set; }
        public int airline_id { get; set; }
    }
}
