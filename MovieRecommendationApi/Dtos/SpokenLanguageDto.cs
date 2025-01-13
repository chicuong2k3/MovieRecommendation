using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Dtos
{
    public class SpokenLanguageDto
    {
        public int Id { get; set; }
        public string? IsoCode { get; set; }
        public string? EnglishName { get; set; }
        public string? Name { get; set; }
    }
}
