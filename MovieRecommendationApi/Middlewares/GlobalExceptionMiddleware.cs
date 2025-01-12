using MovieRecommendationApi.Common;
using System.Net;

namespace MovieRecommendationApi.Middlewares;

public class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = ErrorResponse.Create(new List<string> { ex.Message }, "internal_server_error", HttpStatusCode.InternalServerError);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}