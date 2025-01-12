namespace MovieRecommendationApi.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public bool Adult { get; set; }
        public List<string>? AlsoKnownAs { get; set; }
        public string? Biography { get; set; }
        public string? Birthday { get; set; }
        public string? Deathday { get; set; }
        public int Gender { get; set; }
        public string? Homepage { get; set; }
        public string? KnownForDepartment { get; set; }
        public string? Name { get; set; }
        public string? PlaceOfBirth { get; set; }
        public double Popularity { get; set; }
        public string? ProfilePath { get; set; }
        public int? CreditId { get; set; }
    }
}
