using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.OwnedTicket
{
    public class OwnedTicketUpdateDTO
    {
        public int id { get; set; }
        public decimal Price { get; set; }
        public decimal Baggage_weight { get; set; }
        public int seat_id { get; set; }
        public DateTimeOffset Puchase_date { get; set; }
    }
}
