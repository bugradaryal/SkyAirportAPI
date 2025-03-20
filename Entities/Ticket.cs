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
        public double Price { get; set; }
        public DateTime Puchase_date { get; set; }
        public double Baggage_weight { get; set; }


        public int seet_id { get; set; }
        public int user_id { get; set; }
        public User user { get; set; }
        public Seat seat { get; set; }
    }
}
