using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Seat
    {
        public int id { get; set; }
        public int Seat_number { get; set; }
        public string Seat_Class { get; set; }
        public string Location { get; set; }
        public bool Is_Available { get; set; }


        public int flight_id { get; set; }
        public Flight flight { get;set; }
        public Ticket ticket { get; set; }
    }
}
