using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER_NAME = "X-API-KEY";
    private string _key;

    private static readonly string[] _excludedPaths = new[]
    {
        "/EmailVerification"
    };

    public ApiKeyMiddleware(RequestDelegate next, IOptions<Entities.Configuration.SecurityKey> key)
    {
        _next = next;
        _key = key.Value.ApiKey.ToString();
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (_excludedPaths.Any(p => context.Request.Path.Value.Contains(p, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key gerekli.");
            return;
        }
        if (!_key.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Geçersiz API Key.");
            return;
        }
        await _next(context);
    }
}