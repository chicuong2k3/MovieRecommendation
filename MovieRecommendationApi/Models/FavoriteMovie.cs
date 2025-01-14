using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
    public class FavoriteMovie
    {
        [Key]
        public string Id { get; set; } = default!;
        public string? UserId { get; set; }
        public string? MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
