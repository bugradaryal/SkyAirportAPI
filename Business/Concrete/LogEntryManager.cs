using DataAccess.LogManager;
using Entities;
using Entities.Moderation;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class LogEntryManager
    {
        private readonly DatabaseLogSink _logSink;
        public LogEntryManager(DatabaseLogSink logSink) 
        {
            _logSink = logSink;
        }

        public void Logger(string userId, int LogLevel, string actionType, string message, string tableName = "", int recordId = 0, List<string> additionalData = null)
        {
            // LogEvent oluşturuluyor
            LogEvent logEvent = new LogEvent(
                                        timestamp: DateTimeOffset.UtcNow, 
                                        level:(Serilog.Events.LogEventLevel)LogLevel,
                                        exception: null,
                                        messageTemplate: new MessageTemplate(message, new List<MessageTemplateToken>()),
                                        properties: new List<LogEventProperty>
                                        {
                                             new LogEventProperty("UserId", new ScalarValue(userId)),
                                             new LogEventProperty("ActionType", new ScalarValue(actionType)),
                                             new LogEventProperty("TableName", new ScalarValue(tableName)),
                                             new LogEventProperty("RecordId", new ScalarValue(recordId)),
                                             new LogEventProperty("AdditionalData", new ScalarValue(JsonConvert.SerializeObject(additionalData)))
                                        });
            // Emit metodu çağrılıyor
            _logSink.Emit(logEvent);
        }

    }
}
