using System.Security.Claims;

namespace MovieRecommendationApi
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
		{
			// Lấy giá trị của Claim chứa UID người dùng
			return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
				   ?? throw new InvalidOperationException("User ID not found in claims.");
		}
	}
}

