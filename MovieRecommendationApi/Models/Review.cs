using MovieRecommendationApi.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class Review
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;
        public string? UserId { get; set; }
        public User? User { set; get; }
        public int MovieId { get; set; }
        public Movie? Movie { set; get; }
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
