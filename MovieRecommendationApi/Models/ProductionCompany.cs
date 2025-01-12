using MovieRecommendationApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class ProductionCompany
    {
        [Key]
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("logo_path")]
        public string? LogoPath { get; set; }

        [JsonPropertyName("origin_country")]
        public string? OriginCountry { get; set; }
    }
}
