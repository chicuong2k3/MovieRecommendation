using MovieRecommendationApi.Common;

namespace MovieRecommendationApi.Requests
{
    public record SearchMovieRequest : PaginationRequest
    {
        public SearchMovieRequest(
            int Page,
            int PageSize = 10,
            string? Query = null,
            List<int>? GenreIds = null,
            List<int>? Ratings = null,
            DateTime? ReleaseDateFrom = null,
            DateTime? RealeaseDateTo = null) : base(Page, PageSize)
        {
            this.Query = Query;
            this.GenreIds = GenreIds;
            this.Ratings = Ratings;
            this.ReleaseDateFrom = ReleaseDateFrom;
            this.RealeaseDateTo = RealeaseDateTo;
        }

        public string? Query { get; init; }
        public List<int>? GenreIds { get; init; }
        public List<int>? Ratings { get; init; }

        public DateTime? ReleaseDateFrom { get; init; }
        public DateTime? RealeaseDateTo { get; init; }
    }
}
