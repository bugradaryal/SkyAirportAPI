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
        public int id { get; set; }
        public decimal Price { get; set; }
        public int seat_id { get; set; }
    }
}
