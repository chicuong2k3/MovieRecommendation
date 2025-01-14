using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
    public class WatchMovie
    {
        [Key]
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public User? User { get; set; }
        public string MovieId { get; set; } = default!;
        public Movie? Movie { get; set; }
    }
}
