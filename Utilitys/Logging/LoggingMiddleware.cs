using DTO;
using Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace Utilitys.Logging
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerServices loggerServices)
        {
            var requestPath = context.Request.Path.ToString();
            var requestMethod = context.Request.Method;

            var endpoint = context.GetEndpoint();
            var logAction = endpoint?.Metadata.GetMetadata<LogActionAttribute>();

            // =====================================================
            // LOG ACTION VARSA
            // =====================================================
            if (logAction != null)
            {
                await _next(context);

                await loggerServices.Logger(new LogDTO
                {
                    Message = $"{requestMethod} {requestPath} - {logAction.ActionType}",
                    Action_type = logAction.ActionType,
                    Target_table = requestPath,
                    loglevel_id = LogLevelResolver.FromStatusCode(context.Response.StatusCode),
                    user_id = GetUserId(context)
                });
                return;
            }

            // =====================================================
            // LOG ACTION YOKSA - APIRequest / APIResponse
            // =====================================================
            await loggerServices.Logger(new LogDTO
            {
                Message = $"{requestMethod} {requestPath} called!",
                Action_type = Action_Type.APIRequest,
                Target_table = requestPath,
                loglevel_id = 1,
                user_id = GetUserId(context)
            });

            await _next(context);

            bool isSuccess = context.Response.StatusCode is >= 200 and < 300;
            var message = context.Items.TryGetValue("LogMessage", out var msg) ? msg?.ToString() : null;

            await loggerServices.Logger(new LogDTO
            {
                Message = message ?? $"{requestMethod} {requestPath} " + (isSuccess ? "succeeded" : "failed"),
                Action_type = Action_Type.APIResponse,
                Target_table = requestPath,
                loglevel_id = LogLevelResolver.FromStatusCode(context.Response.StatusCode),
                user_id = GetUserId(context)
            });
        }

        private static string? GetUserId(HttpContext context)
            => context.User?.FindFirst("uid")?.Value;
    }
}