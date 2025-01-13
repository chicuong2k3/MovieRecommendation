namespace MovieRecommendationApi.Common
{
    public class PaginatedResponse<T> where T : class
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public List<T> Results { get; set; } = new();
    }
}
