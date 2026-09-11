namespace Entities
{
    public class AircraftStatus
    {
        public int id { get; set; }
        public string Status { get; set; }
        public ICollection<Aircraft> aircraft { get; set; }
    }
}
