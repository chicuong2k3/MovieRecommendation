using FluentValidation;
using MovieRecommendationApi.Common;
using System.Net;
using System.Text.Json;

namespace MovieRecommendationApi.Middlewares;

public class ValidationMiddleware : IMiddleware
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    public ValidationMiddleware(
        IServiceProvider serviceProvider,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            var requestTypeAttribute = endpoint.Metadata.GetMetadata<RequestTypeAttribute>();

            if (requestTypeAttribute != null)
            {
                var requestType = requestTypeAttribute.RequestType;

                var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
                var validator = (IValidator)_serviceProvider.GetRequiredService(validatorType);


                context.Request.EnableBuffering();
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;
                var request = JsonSerializer.Deserialize(requestBody, requestType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                if (request == null)
                {
                    _logger.LogError("Request body could not be deserialized.");

                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(ErrorResponse.Create(
                        new List<string> { "Request body could not be deserialized." },
                        "invalid_request_body",
                        HttpStatusCode.BadRequest
                    ));
                    return;
                }

                var validationContext = new ValidationContext<object>(request);
                var validationResults = await validator.ValidateAsync(validationContext);

                if (!validationResults.IsValid)
                {
                    _logger.LogError("Request failed validation.");

                    context.Response.StatusCode = 422;
                    var errors = validationResults.Errors.Select(e => e.ErrorMessage).ToList();
                    var response = ErrorResponse.Create(errors, "validation_error", HttpStatusCode.UnprocessableEntity);

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }
            }
        }

        await next(context);
    }


}
