namespace Entities
{
    public class Ticket
    {
        public int id { get; set; }
        public decimal Price { get; set; }
        public int seat_id { get; set; }

        public OwnedTicket ownedTicket { get; set; }
        public Seat seat { get; set; }
    }
}
