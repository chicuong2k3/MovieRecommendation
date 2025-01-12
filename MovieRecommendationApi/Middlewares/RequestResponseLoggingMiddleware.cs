using System.Diagnostics;
using System.Text.Json;

namespace MovieRecommendationApi.Middlewares;

public class RequestResponseLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Request.EnableBuffering();
        var requestLog = new
        {
            context.Request.Method,
            Url = context.Request.Path
        };
        _logger.LogInformation("Incoming Request: {RequestLog}",
            JsonSerializer.Serialize(requestLog, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            var responseLog = new
            {
                context.Response.StatusCode,
                TimeTakenMs = stopwatch.ElapsedMilliseconds
            };
            _logger.LogInformation("Outgoing Response: {ResponseLog}",
                JsonSerializer.Serialize(responseLog, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }
    }
}
