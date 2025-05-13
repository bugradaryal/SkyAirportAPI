using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.OwnedTicket
{
    public class OwnedTicketAddDTO
    {
        public decimal Baggage_weight { get; set; }
        public int ticket_id { get; set; }
        public string user_id { get; set; }
    }
}
