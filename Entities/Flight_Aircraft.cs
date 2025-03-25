using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Flight_Aircraft
    {
        public int id { get; set; }


        public int flight_id { get; set; }
        public int aircraft_id { get; set; }
        public Flight flight { get; set; }
        public Aircraft aircraft { get; set; }
    }
}
