using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.Ticket
{
    public class TicketAddDTO
    {
        public decimal Price { get; set; }
        public int seat_id { get; set; }
    }
}
