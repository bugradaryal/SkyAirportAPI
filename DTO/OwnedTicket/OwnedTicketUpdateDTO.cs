namespace DTO.OwnedTicket
{
    public class OwnedTicketUpdateDTO
    {
        public int id { get; set; }
        public decimal Baggage_weight { get; set; }
        public string user_id { get; set; }
        public int ticket_id { get; set; }
    }
}
