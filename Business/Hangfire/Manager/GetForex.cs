using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Redis;
using DTO;
using Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utilitys.CurrencyService;
using Utilitys.ExceptionHandler;

namespace Business.Hangfire.Manager
{
    public class GetForex
    {
        private readonly IRedisServices _redisService;
        private readonly ILoggerServices _loggerServices;

        public GetForex(IRedisServices redisService, ILoggerServices loggerServices)
        {
            _loggerServices = loggerServices;
            _redisService = redisService;
        }
        public async Task Run()
        {
            try{
                await _loggerServices.Logger(new LogDTO
                {
                    Message = "BackgroundJob starting!",
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Hangfire",
                    loglevel_id = 1,
                }, null);
                var list = await ForexManager.CurrencyGetter();

                string redisKey = "forex";

                var json = JsonConvert.SerializeObject(list, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                await _redisService.SetAsync(redisKey, json);

                await _loggerServices.Logger(new LogDTO
                {
                    Message = "Current forex:    "+ json,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Hangfire",
                    loglevel_id = 1,
                }, null);
            }
            catch(Exception ex) 
            {
                await _loggerServices.Logger(new LogDTO
                {
                    Message =  "Exception throw!",
                    Action_type = Action_Type.SystemError,
                    Target_table = "Hangfire",
                    loglevel_id = 5,
                }, new CustomException(ex.Message,4,(int)HttpStatusCode.BadRequest,ex.InnerException?.Message));
            }

        }
    }
}
