using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Dtos
{
    public class VideoDto
    {
        public string Id { get; set; } = default!;
        public string? Name { get; set; }
        public string? Key { get; set; }
        public string? Site { get; set; }
        public int Size { get; set; }
        public string? Type { get; set; }
        public bool Official { get; set; }
        public DateTime? PublishedAt { get; set; }

        public string? MovieId { set; get; }
    }
}
