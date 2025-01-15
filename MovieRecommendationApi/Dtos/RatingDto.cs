namespace MovieRecommendationApi.Dtos
{
    public class RatingDto
    {
        public MovieDto? Movie { get; set; }
        public int Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
