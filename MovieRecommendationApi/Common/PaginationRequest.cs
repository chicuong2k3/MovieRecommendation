﻿namespace MovieRecommendationApi.Common
{
    public record PaginationRequest(int Page = 1, int PageSize = 10);
}
