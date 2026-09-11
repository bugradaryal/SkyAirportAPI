namespace Entities.Moderation
{
    public class LogLevel
    {
        public int id { get; set; }
        public string Level { get; set; }


        public ICollection<LogEntry> logEntry { get; set; }
    }
}
