using MovieRecommendationApi.Models;

namespace MovieRecommendationApi.Dtos
{
    public class ReviewDto
    {
        public string Id { get; set; } = default!;
        public string? UserId { get; set; }
        public string MovieId { get; set; } = default!;
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Url { get; set; }
        public int Rating { get; set; }

        public UserDto? AuthorDetails { get; set; }
    }
}
