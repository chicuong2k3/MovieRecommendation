namespace MovieRecommendationApi.Models
{
    public class MovieCast
    {
        public int MovieId { set; get; }
        public Movie Movie { set; get; }
        public int CastId { set; get; }
        public Person Cast { set; get; }
    }
}
