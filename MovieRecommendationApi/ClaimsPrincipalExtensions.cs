using System.Security.Claims;

namespace MovieRecommendationApi
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return new Guid("a857cbb5-996e-49d5-ae7d-20d943f2ebcd").ToString();
            //return claimsPrincipal.FindFirstValue("sub")
            //    ?? throw new InvalidOperationException("Invalid User Id");
        }
    }
}
