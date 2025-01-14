using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class SimilarMovie
    {
        public string MovieId { get; set; } = default!;
        public Movie Movie { get; set; } = default!;
        public string SimilarMovieId { get; set; } = default!;
        public Movie SimilarMovieEntity { get; set; } = default!;

    }
}
