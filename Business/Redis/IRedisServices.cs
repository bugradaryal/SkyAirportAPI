using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Business.Redis
{
    public interface IRedisServices
    {
        IDatabase GetDb();
        Task SetAsync(string key, string value);
        Task<string> GetAsync(string key);
    }
}
