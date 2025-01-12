using System.Dynamic;
using System.Reflection;

namespace MovieRecommendationApi.DataShaping;

public static class IEnumerableExtensions
{
    public static IEnumerable<ExpandoObject> ShapeData<T>(
        this IEnumerable<T> source,
        string? fields)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        List<ExpandoObject> result = [];

        List<PropertyInfo> propertyInfoList = [];

        if (string.IsNullOrWhiteSpace(fields))
        {
            var properyInfos = typeof(T).GetProperties(
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);

            propertyInfoList.AddRange(properyInfos);
        }
        else
        {
            var splittedFields = fields.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in splittedFields)
            {
                var propertyName = field.Trim();

                var properyInfo = typeof(T).GetProperty(propertyName,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);

                if (properyInfo is null)
                {
                    throw new Exception($"Property {propertyName} not found on {typeof(T)}");
                }

                propertyInfoList.Add(properyInfo);
            }
        }

        foreach (var sourceObject in source)
        {
            var dataShapedObject = new ExpandoObject();

            foreach (var propertyInfo in propertyInfoList)
            {
                var propertyValue = propertyInfo.GetValue(sourceObject);

                ((IDictionary<string, object?>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
            }

            result.Add(dataShapedObject);
        }

        return result;
    }

}