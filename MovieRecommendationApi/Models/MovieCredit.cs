using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class MovieCredit
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("cast")]
        public List<Movie>? Cast { get; set; }
    }
}
