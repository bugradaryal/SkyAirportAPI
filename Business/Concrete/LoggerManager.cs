using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DTO;
using DTO.Account;
using Entities;
using Entities.Enums;
using Entities.Moderation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Events;
using Utilitys.ExceptionHandler;
using Utilitys.Logger;
using Utilitys.Mapper;

namespace Business.Concrete
{
    public class LoggerManager : ILoggerServices
    {
        private readonly ISerilogServices _logger;
        private readonly IMapper _mapper;
        private readonly ILogRepository _logRepository;
        public LoggerManager(IMapper mapper)
        {
            _mapper = mapper;
            _logger = new SerilogLogger();
            _logRepository = new LogRepository();
        }
        public async Task Logger(LogDTO logdto, CustomException? exception = null)
        {
            try
            {
                var logData = _mapper.Map<LogEntry, LogDTO>(logdto);
                if (logdto.loglevel_id < 3)
                {
                    _logger.Info(logdto);
                    await _logRepository.AddLog(logData);
                }
                else if(logdto.loglevel_id == 3)
                {
                    _logger.Warn(logdto);
                    await _logRepository.AddLog(logData);
                }
                else
                {
                    logdto.Message += $"       /       {JsonConvert.SerializeObject(exception)}";
                    _logger.Error(logdto, exception);
                    await _logRepository.AddLog(logData);
                }
            }
            catch (Exception ex) 
            {
                _logger.Fatal(logdto, ex);
                await _logRepository.AddLog(new LogEntry {  
                    Action_type = Action_Type.SystemError, 
                    loglevel_id = (int)LogEventLevel.Fatal,
                    Message = "Critical Fatal Error!!       /       " + JsonConvert.SerializeObject(ex),
                    Target_table = logdto.Target_table,
                    user_id = logdto.user_id,
                    AdditionalData = logdto.AdditionalData
                });
            }

        }
    }
}
