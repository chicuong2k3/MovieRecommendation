namespace MovieRecommendationApi.Dtos
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string AvatarPath { get; set; } = string.Empty;
    }
}
