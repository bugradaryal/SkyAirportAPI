namespace DTO
{
    public class SeatDTO
    {
        public int id { get; set; }
        public int Seat_number { get; set; }
        public string Seat_Class { get; set; }
        public string Location { get; set; }
        public bool Is_Available { get; set; }
        public int flight_id { get; set; }
    }
}
