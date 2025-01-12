namespace MovieRecommendationApi.Dtos
{
    public class CreditDto
    {
        public int Id { get; set; }
        public List<PersonDto>? Cast { get; set; }
    }
}
