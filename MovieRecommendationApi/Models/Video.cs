using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class Video
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("site")]
        public string? Site { get; set; }
        [JsonPropertyName("size")]
        public int Size { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("official")]
        public bool Official { get; set; }
        [JsonPropertyName("published_at")]
        public DateTime? PublishedAt { get; set; }

        public string? MovieId { set; get; }
        public Movie? Movie { get; set; }
    }
}
