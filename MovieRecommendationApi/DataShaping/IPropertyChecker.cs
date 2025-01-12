namespace MovieRecommendationApi.DataShaping
{
    public interface IPropertyChecker
    {
        bool TypeHasProperties<T>(string? fields);
        bool TypeHasProperties(Type type, string? fields);
    }
}
