using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.OwnedTicket
{
    public class OwnedTicketAddDTO
    {
        public decimal Price { get; set; }
        public decimal Baggage_weight { get; set; }
        public int seat_id { get; set; }
    }
}
