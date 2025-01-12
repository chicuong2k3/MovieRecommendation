using System.Dynamic;
using System.Reflection;

namespace MovieRecommendationApi.DataShaping;

public static class ObjectExtensions
{
    public static ExpandoObject ShapeData<T>(
        this T source,
        string? fields)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

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

        var dataShapedObject = new ExpandoObject();

        foreach (var propertyInfo in propertyInfoList)
        {
            var propertyValue = propertyInfo.GetValue(source);

            ((IDictionary<string, object?>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
        }


        return dataShapedObject;
    }
}
