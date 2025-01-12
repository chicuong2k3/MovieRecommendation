using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
    public class WatchMovie
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
