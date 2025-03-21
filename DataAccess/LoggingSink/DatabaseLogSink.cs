using Entities.Moderation;
using Serilog.Core;
using Serilog.Events;

namespace DataAccess.LoggingSink
{
    public class DatabaseLogSink : ILogEventSink
    {
        private readonly DataDbContext _context;

        public DatabaseLogSink(DataDbContext context)
        {
            _context = context;
        }

        public void Emit(LogEvent logEvent)
        {
            // LogEntry oluşturuyoruz
            var logEntry = new LogEntry
            {
                Timestamp = logEvent.Timestamp.UtcDateTime,
                Log_level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(),
                Action_type = logEvent.Properties.ContainsKey("ActionType") ? logEvent.Properties["ActionType"].ToString() : null,
                Table_name = logEvent.Properties.ContainsKey("TableName") ? logEvent.Properties["TableName"].ToString() : null,
                Record_id = logEvent.Properties.ContainsKey("RecordId") ? Convert.ToInt32(logEvent.Properties["RecordId"]) : 0,
                AdditionalData = logEvent.Properties.ContainsKey("AdditionalData")
                ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(logEvent.Properties["AdditionalData"].ToString())
                : new List<string>()
            };

            // LogEntry'yi veritabanına ekliyoruz
            _context.LogEntrys.Add(logEntry);
            _context.SaveChanges();
        }
    }
}
