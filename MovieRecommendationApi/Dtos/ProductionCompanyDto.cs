using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Dtos
{
    public class ProductionCompanyDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LogoPath { get; set; }
        public string? OriginCountry { get; set; }
    }
}
