using Entities.Enums;
using Entities.Moderation;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;

namespace DataAccess.LogManager
{
    public class DatabaseLogSink : ILogEventSink
    {
        private readonly DataDbContext _context;

        public DatabaseLogSink(DataDbContext context)
        {
            _context = context;
        }

        // Emit metodunda logları veritabanına ekliyoruz

        public async void Emit(LogEvent logEvent)
        {

            // Log seviyesini logEvent'den alıyoruz
            int logLevel = (int)logEvent.Level;

            // Log seviyesini LogLevel tablosundan alıyoruz
            var logLevelEntity = _context.LogLevels
                .FirstOrDefault(l => l.id == logLevel);
            string userId = logEvent.Properties.ContainsKey("user_id") ? logEvent.Properties["user_id"].ToString() : null;
            var logEntry = new LogEntry
            {
                Timestamp = logEvent.Timestamp.UtcDateTime,
                Message = logEvent.RenderMessage(),
                Action_type = logEvent.Properties.ContainsKey("ActionType")
                    ? Enum.Parse<Action_Type>(logEvent.Properties["ActionType"].ToString())
                    : Action_Type.SystemError, // Varsayılan değer
                Target_table = logEvent.Properties.ContainsKey("TableName")
                    ? logEvent.Properties["TableName"].ToString()
                    : string.Empty,
                Record_id = logEvent.Properties.ContainsKey("RecordId")
                    ? Convert.ToInt32(logEvent.Properties["RecordId"])
                    : 0,
                AdditionalData = logEvent.Properties.ContainsKey("AdditionalData")
                    ? JsonConvert.DeserializeObject<List<string>>(logEvent.Properties["AdditionalData"].ToString())
                    : new List<string>(),
                user_id = userId,  // Örnek olarak sabit bir kullanıcı ID'si
                loglevel_id = logLevelEntity?.id ?? 1  // LogLevel tablosundaki ID'yi alıyoruz, yoksa 1'i kullanıyoruz
            };

            _context.LogEntrys.Add(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}
