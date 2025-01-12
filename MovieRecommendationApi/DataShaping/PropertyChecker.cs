using System.Reflection;

namespace MovieRecommendationApi.DataShaping
{
    public class PropertyChecker : IPropertyChecker
    {
        public bool TypeHasProperties<T>(string? fields)
        {
            return TypeHasProperties(typeof(T), fields);
        }
        public bool TypeHasProperties(Type type, string? fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var splittedFields = fields.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in splittedFields)
            {
                var propertyName = field.Trim();

                var properyInfo = type.GetProperty(propertyName,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);

                if (properyInfo is null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
