using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class Movie
    {
        [Key]
        [JsonPropertyName("tmdb_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }

        [JsonPropertyName("belongs_to_collection")]
        public BelongsToCollection? BelongsToCollection { get; set; }

        [JsonPropertyName("budget")]
        public int Budget { get; set; }

        [JsonPropertyName("categories")]
        public IList<string>? Categories { get; set; }

        [JsonPropertyName("genres")]
        public IList<Genre>? Genres { get; set; }

        [JsonPropertyName("homepage")]
        public string? Homepage { get; set; }

        [JsonPropertyName("origin_country")]
        public IList<string>? OriginCountry { get; set; }

        [JsonPropertyName("original_language")]
        public string? OriginalLanguage { get; set; }

        [JsonPropertyName("original_title")]
        public string? OriginalTitle { get; set; }

        [JsonPropertyName("overview")]
        public string? Overview { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("production_companies")]
        public IList<ProductionCompany>? ProductionCompanies { get; set; }

        [JsonPropertyName("production_countries")]
        public IList<ProductionCountry>? ProductionCountries { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        //[JsonPropertyName("revenue")]
        //public long? Revenue { get; set; }

        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }

        [JsonPropertyName("spoken_languages")]
        public IList<SpokenLanguage>? SpokenLanguages { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("tagline")]
        public string? Tagline { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }

        [JsonPropertyName("credits")]
        public Credit? Credits { get; set; }

        [JsonPropertyName("movie_casts")]
        public IList<MovieCast>? MovieCasts { get; set; }

        [JsonPropertyName("rating_lists")]
        public IList<RatingList>? RatingLists { get; set; }

        [JsonPropertyName("reviews")]
        public IList<Review>? Reviews { get; set; }

        [JsonPropertyName("watch_movies")]
        public IList<WatchMovie>? WatchMovies { get; set; }

        [JsonPropertyName("favorite_movies")]
        public IList<FavoriteMovie>? FavoriteMovies { get; set; }

		public IList<Video>? Videos { get; set; }
	}
}
