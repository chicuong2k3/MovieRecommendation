namespace MovieRecommendationApi.Models
{
    public class RatingList
    {

        public string UserId { get; set; } = default!;
        public User? User { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int Rating { get; set; }
    }
}
