using MovieRecommendationApi.Models;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Data
{
    public class SimilarMovieModel
    {


        [JsonPropertyName("tmdb_id")]
        public int TmdbId { get; set; }


        [JsonPropertyName("similar_movies")]
        public List<Movie> SimilarMovies { get; set; } = [];
    }
}
