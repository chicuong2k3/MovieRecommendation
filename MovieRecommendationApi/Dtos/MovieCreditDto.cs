using MovieRecommendationApi.Models;

namespace MovieRecommendationApi.Dtos
{
    public class MovieCreditDto
    {
        public int Id { get; set; }
        public List<MovieDto>? Cast { get; set; }
    }
}
