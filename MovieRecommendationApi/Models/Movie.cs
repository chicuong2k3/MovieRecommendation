using MovieRecommendationApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class Movie
    {
        [Key]
        [JsonPropertyName("_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public string Id { get; set; } = default!;
        [JsonPropertyName("imdb_id")]
        public string? ImdbId { get; set; }
        [JsonPropertyName("tmdb_id")]
        public int? TmdbId { get; set; }

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
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? ReleaseDate { get; set; }

        //[JsonPropertyName("revenue")]
        //public int Revenue { get; set; }

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

        [JsonPropertyName("reviews")]
        [NotMapped]
        public IList<ReviewModel>? ReviewModels { get; set; }

		[JsonIgnore]
		public IList<Review>? Reviews { get; set; }

        public IList<WatchMovie>? WatchMovies { get; set; }
        public IList<FavoriteMovie>? FavoriteMovies { get; set; }
        [JsonPropertyName("trailers")]
        public IList<Video>? Videos { get; set; }
    }
}
