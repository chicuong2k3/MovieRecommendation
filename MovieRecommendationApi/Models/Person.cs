using MovieRecommendationApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class Person
    {
        [JsonPropertyName("_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public string Id { get; set; } = default!;

        [JsonPropertyName("id")]
        public int IdForCrawling { get; set; }

        [JsonPropertyName("adult")]
        public bool Adult { get; set; }
        [JsonPropertyName("imdb_id")]
        public string? ImdbId { get; set; }
        [JsonPropertyName("tmdb_id")]
        public int? TmdbId { get; set; }

        [JsonPropertyName("also_known_as")]
        public List<string>? AlsoKnownAs { get; set; }

        [JsonPropertyName("biography")]
        public string? Biography { get; set; }

        [JsonPropertyName("birthday")]
        public string? Birthday { get; set; }

        [JsonPropertyName("deathday")]
        public string? Deathday { get; set; }

        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        [JsonPropertyName("homepage")]
        public string? Homepage { get; set; }

        [JsonPropertyName("known_for_department")]
        public string? KnownForDepartment { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("place_of_birth")]
        public string? PlaceOfBirth { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }

        [JsonPropertyName("movie_credits")]
        public MovieCredit? MovieCredits { get; set; }
    }
}
