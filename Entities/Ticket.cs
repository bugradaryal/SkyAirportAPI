using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Ticket
    {
        public int id { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset Puchase_date { get; set; }
        public decimal Baggage_weight { get; set; }


        public int seat_id { get; set; }
        public string user_id { get; set; }
        public User user { get; set; }
        public Seat seat { get; set; }
    }
}
