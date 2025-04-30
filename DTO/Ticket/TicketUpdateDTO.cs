using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class TicketUpdateDTO
    {
        [Required]
        public int id { get; set; }

        [Range(0, 99999999.99)]
        public decimal Price { get; set; }

        [Range(0, 999999.99)]
        public decimal Baggage_weight { get; set; }
        public int seat_id { get; set; }
        public DateTimeOffset Puchase_date { get; set; }
    }
}
