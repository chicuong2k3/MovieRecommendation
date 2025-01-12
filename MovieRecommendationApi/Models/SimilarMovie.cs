using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class SimilarMovie
    {
        [JsonPropertyName("tmdb_id")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = default!;

        [JsonPropertyName("similar_movie_id")]
        public int SimilarMovieId { get; set; }
        public Movie SimilarMovieEntity { get; set; } = default!;

    }
}
