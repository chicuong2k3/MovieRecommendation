using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MovieRecommendationApi.Common;

public class ErrorResponse
{
    public List<string> Errors { get; }
    public DateTime Timestamp { get; }
    public string? ErrorCode { get; }
    public HttpStatusCode StatusCode { get; }

    private ErrorResponse(List<string> errors, string? errorCode = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Errors = errors ?? new List<string>();
        Timestamp = DateTime.UtcNow;
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    public static ErrorResponse Create(
        IEnumerable<string> errors,
        string? errorCode = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ErrorResponse(errors.ToList(), errorCode, statusCode);
    }

    public static ErrorResponse Create(
        string error,
        string? errorCode = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ErrorResponse(new List<string> { error }, errorCode, statusCode);
    }
}

public static class ResultExtensions
{
    public static IActionResult MapErrorResponse(this ErrorResponse? errorResponse)
    {
        if (errorResponse == null || !errorResponse.Errors.Any())
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return new ObjectResult(errorResponse)
        {
            StatusCode = (int)errorResponse.StatusCode
        };
    }
}