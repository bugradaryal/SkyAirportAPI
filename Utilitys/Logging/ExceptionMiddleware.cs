using DTO;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Utilitys.Logging.ExceptionHandler;

namespace Utilitys.Logging
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerServices loggerServices)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                int statusCode = ex is CustomException customEx
                    ? customEx.ErrorCode
                    : StatusCodes.Status500InternalServerError;

                await loggerServices.Logger(new LogDTO
                {
                    Message = ex.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = context.Request.Path,
                    loglevel_id = LogLevelResolver.FromStatusCode(statusCode),
                    user_id = context.User?.FindFirst("uid")?.Value
                }, new CustomException(ex.Message, StatusCodes.Status500InternalServerError));

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var errorResponse = new
                {
                    message = ex.Message,
                    errorCode = statusCode.ToString()
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
}