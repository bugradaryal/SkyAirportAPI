using DataAccess.Abstract;
using DTO;
using Entities.Enums;
using Entities.Moderation;
using Newtonsoft.Json;
using Utilitys.Logging.ExceptionHandler;
using Utilitys.Logging.Serilog;
using Utilitys.Mapper;

namespace Utilitys.Logging
{
    public class LoggerManager : ILoggerServices
    {
        private readonly ISerilogServices _logger;
        private readonly IMapper _mapper;
        private readonly ILogRepository _logRepository;
        public LoggerManager(IMapper mapper, ISerilogServices logger, ILogRepository logRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _logRepository = logRepository;
        }
        public async Task Logger(LogDTO logdto, CustomException? exception = null)
        {
            var logData = _mapper.Map<LogEntry, LogDTO>(logdto);
            try
            {
                if (logData.loglevel_id == 1)
                {
                    _logger.Info(logData);
                    await _logRepository.AddLog(logData);
                }
                else if (logData.loglevel_id == 2)
                {
                    _logger.Warn(logData);
                    await _logRepository.AddLog(logData);
                }
                else
                {
                    logData.Message += $"Error!!       /       {JsonConvert.SerializeObject(exception)}";
                    _logger.Error(logData, exception);
                    await _logRepository.AddLog(logData);
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(logData, ex);
                await _logRepository.AddLog(new LogEntry
                {
                    Action_type = Action_Type.SystemError,
                    loglevel_id = 4,
                    Message = "Critical Fatal Error!!       /       " + JsonConvert.SerializeObject(ex),
                    Target_table = logData.Target_table,
                    user_id = logData.user_id ?? null,
                    AdditionalData = logData.AdditionalData
                });
            }

        }
    }
}
