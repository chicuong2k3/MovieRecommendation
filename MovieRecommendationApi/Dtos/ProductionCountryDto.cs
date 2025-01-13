using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Dtos
{
    public class ProductionCountryDto
    {
        public int Id { get; set; }
        public string? IsoCode { get; set; }
        public string? Name { get; set; }
    }
}
