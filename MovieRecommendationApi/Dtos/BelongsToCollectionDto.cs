using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Dtos
{
    public class BelongsToCollectionDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
    }
}
