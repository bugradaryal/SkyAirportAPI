using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Redis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utilitys.CurrencyService;

namespace Business.Hangfire.Manager
{
    public class GetForex
    {
        private readonly IRedisServices _redisService;

        public GetForex(IRedisServices redisService)
        {
            _redisService = redisService;
        }
        public async Task Run()
        {
            var list = ForexManager.CurrencyGetter();

            string redisKey = "forex";
            var json = JsonConvert.SerializeObject(list, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await _redisService.SetAsync(redisKey, json);
        }
    }
}
