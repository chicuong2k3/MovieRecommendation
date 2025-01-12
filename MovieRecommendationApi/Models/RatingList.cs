namespace MovieRecommendationApi.Models
{
    public class RatingList
    {

        public string UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public double Rating { get; set; }
    }
}
