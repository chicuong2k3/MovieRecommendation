using MovieRecommendationApi.Common;

namespace MovieRecommendationApi.Requests
{
    public record SearchMovieRequest : PaginationRequest
    {
        public SearchMovieRequest(int Page, int PageSize = 10, string? Query = null) : base(Page, PageSize)
        {
            this.Query = Query;
        }

        public string? Query { get; init; }
    }
}
