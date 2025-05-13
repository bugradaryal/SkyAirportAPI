using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OwnedTicket
    {
        public int id { get; set; }
        public DateTimeOffset Puchase_date { get; set; }
        public decimal Baggage_weight { get; set; }

        public string user_id { get; set; }
        public int ticket_id { get; set; }
        public User user { get; set; }
        public Ticket ticket { get; set; }
    }
}
