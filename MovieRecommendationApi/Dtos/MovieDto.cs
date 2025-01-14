using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Dtos
{
    public class MovieDto
    {
        public string Id { get; set; } = default!;
        public bool Adult { get; set; }
        public string? ImdbId { get; set; }
        public int? TmdbId { get; set; }
        public string? BackdropPath { get; set; }
        public BelongsToCollectionDto? BelongsToCollection { get; set; }
        public int Budget { get; set; }
        public List<string>? Categories { get; set; }
        public List<GenreDto>? Genres { get; set; }
        public string? Homepage { get; set; }
        public List<string>? OriginCountry { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public double Popularity { get; set; }
        public string? PosterPath { get; set; }
        public List<ProductionCompanyDto>? ProductionCompanies { get; set; }
        public List<ProductionCountryDto>? ProductionCountries { get; set; }
        public DateTime? ReleaseDate { get; set; }
        //public int Revenue { get; set; }
        public int Runtime { get; set; }
        public List<SpokenLanguageDto>? SpokenLanguages { get; set; }
        public string? Status { get; set; }
        public string? Tagline { get; set; }
        public string? Title { get; set; }
        public bool Video { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }

    }
}
