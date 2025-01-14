namespace MovieRecommendationApi.Requests
{
    public record AddReviewRequest(int Rating, string Name, string Username, string? AvatarPath, string? Content);
}
