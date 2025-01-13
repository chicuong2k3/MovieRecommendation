using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
    public class MovieCredit
    {
        [Key]
        public int Id { get; set; }
        public List<Movie>? Cast { get; set; }
    }
}
