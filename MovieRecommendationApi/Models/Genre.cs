using MovieRecommendationApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class Genre
    {
        [Key]
        [JsonPropertyName("_id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = default!;

        [JsonPropertyName("id")]
        public int IdForCrawling { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("tmdb_id")]
        public int TmdbId { get; set; }

        public IList<Movie>? Movies { get; set; }
    }
}
