namespace MovieRecommendationApi.Common
{
    public record PaginationRequest(int Page, int PageSize = 10);
}
