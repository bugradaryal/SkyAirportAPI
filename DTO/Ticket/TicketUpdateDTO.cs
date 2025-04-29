using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class TicketUpdateDTO
    {
        public int id { get; set; }
        public double Price { get; set; }
        public DateTimeOffset Puchase_date { get; set; }
        public double Baggage_weight { get; set; }
        public int seat_id { get; set; }
    }
}
