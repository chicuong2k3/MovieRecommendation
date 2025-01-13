using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class SimilarMovie
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = default!;
        public int SimilarMovieId { get; set; }
        public Movie SimilarMovieEntity { get; set; } = default!;

    }
}
