using System.Security.Claims;

namespace MovieRecommendationApi.Controllers.AuthenFirebase
{
	public class FirebaseAuthenticationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly FirebaseTokenVerifier _tokenVerifier;

		public FirebaseAuthenticationMiddleware(RequestDelegate next, FirebaseTokenVerifier tokenVerifier)
		{
			_next = next;
			_tokenVerifier = tokenVerifier;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (string.IsNullOrEmpty(token))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Authorization token is missing.");
				return;
			}

			try
			{
				// Xác thực token
				var decodedToken = await _tokenVerifier.VerifyIdTokenAsync(token);

				// Tạo ClaimsPrincipal từ thông tin người dùng
				var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid)
			};

				// Lưu principal vào context
				context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "firebase"));

				// Tiếp tục chuỗi middleware
				await _next(context);
			}
			catch (Exception ex)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync($"Unauthorized: {ex.Message}");
			}
		}
	}
}
