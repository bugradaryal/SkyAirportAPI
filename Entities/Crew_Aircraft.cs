namespace Entities
{
    public class Crew_Aircraft
    {
        public int id { get; set; }


        public int crew_id { get; set; }
        public int aircraft_id { get; set; }
        public Aircraft aircraft { get; set; }
        public Crew crew { get; set; }
    }
}
