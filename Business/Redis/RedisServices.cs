using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace Business.Redis
{
    public class RedisServices : IRedisServices
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly IDatabase _db;
        public RedisServices(IConfiguration config)
        {
            var connectionString = config.GetValue<string>("Redis:ConnectionString");
            _connection = ConnectionMultiplexer.Connect(connectionString);
            _db = _connection.GetDatabase();
        }

        public IDatabase GetDb()
        {
            return _db;
        }
        public async Task SetAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        public async Task<string> GetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

    }
}
