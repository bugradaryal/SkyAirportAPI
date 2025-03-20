using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Flight
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Arrival_Date { get; set; }
        public DateTime Deperture_Date { get; set; }
        public string Status { get; set; }


        public int airline_id { get; set; }
        public ICollection<Seat> seat { get; set; }
        public Airline airline { get; set; }
        public OperationalDelay operationalDelay { get; set; }
        public Flight_Aircraft flight_Aircraft { get; set; }

    }
}
