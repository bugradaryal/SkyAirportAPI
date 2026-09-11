namespace DTO.OperationalDelay
{
    public class OperationalDelayAddDTO
    {
        public string Delay_Reason { get; set; }
        public string Delay_Duration { get; set; }
        public DateTimeOffset Delay_Time { get; set; }
        public int flight_id { get; set; }
    }
}
