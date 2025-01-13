using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Data
{
    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();

            // Try parsing the date string into DateTime
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var result))
            {
                // Ensure the result is in UTC
                return result.Kind == DateTimeKind.Utc ? result : result.ToUniversalTime();
            }

            return null; // Or throw an exception based on your needs
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            // Ensure that the DateTime is in UTC before serialization
            var utcDate = value?.ToUniversalTime();
            writer.WriteStringValue(utcDate?.ToString("yyyy-MM-dd"));
        }
    }
}
