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
            int? ReleaseYearFrom = null,
            int? ReleaseYearTo = null) : base(Page, PageSize)
        {
            this.Query = Query;
            this.GenreIds = GenreIds;
            this.Ratings = Ratings;
            this.ReleaseYearFrom = ReleaseYearFrom;
            this.ReleaseYearTo = ReleaseYearTo;
        }

        public string? Query { get; init; }
        public List<int>? GenreIds { get; init; }
        public List<int>? Ratings { get; init; }

        public int? ReleaseYearFrom { get; init; }
        public int? ReleaseYearTo { get; init; }
    }
}
