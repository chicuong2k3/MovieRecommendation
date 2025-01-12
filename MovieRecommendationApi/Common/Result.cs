using System.Net;

namespace MovieRecommendationApi.Common;
public class Result<T> where T : class
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public ErrorResponse? ErrorResponse { get; }
    protected Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }
    protected Result(ErrorResponse errorResponse)
    {
        IsSuccess = false;
        ErrorResponse = errorResponse;
    }
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(
        string error,
        string errorCode,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(ErrorResponse.Create(error, errorCode, statusCode));
}

public class Result : Result<object>
{
    protected Result(object value) : base(value)
    {
    }

    protected Result(ErrorResponse errorResponse) : base(errorResponse)
    {
    }
    public static Result Success() => new(new object());
    public static new Result Failure(
        string error,
        string errorCode,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(ErrorResponse.Create(error, errorCode, statusCode));
}