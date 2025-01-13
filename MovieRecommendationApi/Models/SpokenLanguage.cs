using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class SpokenLanguage
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("iso_639_1")]
        public string? IsoCode { get; set; }

        [JsonPropertyName("english_name")]
        public string? EnglishName { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
