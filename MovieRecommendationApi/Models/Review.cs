namespace MovieRecommendationApi.Models
{
    public class Review
    {
        public string UserId { get; set; }
        public User User { set; get; }
        public int MovieId { get; set; }
        public Movie Movie { set; get; }
        public string Content { get; set; }
    }
}
