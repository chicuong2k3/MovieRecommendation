

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Data;

public class NullableIntConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null; // Handle null values
        }

        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int value))
        {
            return value; // Handle valid integers
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString()!;
            if (int.TryParse(stringValue, out int parsedValue))
            {
                return parsedValue; // Handle numeric strings
            }
        }

        return null; // Fallback for unsupported formats
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteNumberValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
