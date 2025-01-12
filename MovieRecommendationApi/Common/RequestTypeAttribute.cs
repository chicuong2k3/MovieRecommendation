namespace MovieRecommendationApi.Common;


[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class RequestTypeAttribute : Attribute
{
    public Type RequestType { get; }

    public RequestTypeAttribute(Type requestType)
    {
        RequestType = requestType;
    }
}