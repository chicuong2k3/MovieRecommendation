using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class ProductionCountry
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("iso_3166_1")]
        public string? IsoCode { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public IList<Movie>? Movies { get; set; }
    }
}
