using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Business.Abstract;
using DTO;
using Entities.Enums;
using System.Net;
using Utilitys.ExceptionHandler;
using Newtonsoft.Json.Linq;

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
            _connectionString = config.GetValue<string>("Redis:ConnectionString");
        }
        public async Task SetAsync(string key, string value)
        {
            try
            {
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Redis trying to connect!",
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Redis",
                    loglevel_id = 1,
                }, null);
                var _connection = ConnectionMultiplexer.Connect(_connectionString);
                var _db = _connection.GetDatabase();
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Redis Connected",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Redis",
                    loglevel_id = 1,
                }, null);
                await _db.StringSetAsync(key, value);
            }
            catch (Exception ex)
            {
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Exception Throw",
                    Action_type = Action_Type.SystemError,
                    Target_table = "Redis",
                    loglevel_id = 5,
                }, new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message));
            }

        }

        public async Task<string> GetAsync(string key)
        {
            try
            {
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Redis trying to connect!",
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Redis",
                    loglevel_id = 1,
                }, null);
                var _connection = ConnectionMultiplexer.Connect(_connectionString);
                var _db = _connection.GetDatabase();
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Redis Connected",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Redis",
                    loglevel_id = 1,
                }, null);
                return await _db.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Exception Throw",
                    Action_type = Action_Type.SystemError,
                    Target_table = "Redis",
                    loglevel_id = 5,
                }, new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message));
                return null;
            }
        }

    }
}
