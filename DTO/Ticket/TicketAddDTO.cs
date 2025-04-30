using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.Ticket
{
    public class TicketAddDTO
    {
        [Required]
        [Range(0, 99999999.99)] 
        public decimal Price { get; set; }

        [Required]
        [Range(0, 999999.99)] 
        public decimal Baggage_weight { get; set; }

        [Required]
        public int seat_id { get; set; }
    }
}
