using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Business.Abstract;
using DTO;
using Entities.Enums;
using System.Net;
using Newtonsoft.Json.Linq;
using Utilitys.Logging;
using Utilitys.Logging.ExceptionHandler;

namespace Business.Redis
{
    public class RedisServices : IRedisServices
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly IDatabase _db;
        private readonly ILoggerServices _loggerServices;
        private readonly string _connectionString;
        public RedisServices(IConfiguration config, ILoggerServices loggerServices)
        {
            _loggerServices = loggerServices;

            var connectionString = config.GetValue<string>("Redis:ConnectionString");

            var options = ConfigurationOptions.Parse(connectionString);
            options.AbortOnConnectFail = false;

            var connection = ConnectionMultiplexer.Connect(options);
            _db = connection.GetDatabase();
        }
        public async Task SetAsync(string key, string value)
        {
            try
            {
                await _db.StringSetAsync(key, value);
            }
            catch (Exception ex)
            {
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Redis Exception Throw",
                    Action_type = Action_Type.SystemError,
                    Target_table = "Redis",
                    loglevel_id=3
                }, new CustomException(ex.Message, (int)HttpStatusCode.InternalServerError, ex.InnerException?.Message));
            }

        }

        public async Task<string?> GetAsync(string key)
        {
            try
            {
                return await _db.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Redis Exception Throw",
                    Action_type = Action_Type.SystemError,
                    Target_table = "Redis",
                    loglevel_id=3
                }, new CustomException(ex.Message, (int)HttpStatusCode.InternalServerError, ex.InnerException?.Message));
                return null;
            }
        }

    }
}
