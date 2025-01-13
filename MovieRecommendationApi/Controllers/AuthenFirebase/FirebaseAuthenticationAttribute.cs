	

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieRecommendationApi.Controllers.AuthenFirebase
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class FirebaseAuthenticationAttribute : TypeFilterAttribute
	{
		public FirebaseAuthenticationAttribute() : base(typeof(FirebaseAuthenticationFilter))
		{
		}

		private class FirebaseAuthenticationFilter : IAsyncActionFilter
		{
			private readonly FirebaseTokenVerifier _tokenVerifier;

			public FirebaseAuthenticationFilter(FirebaseTokenVerifier tokenVerifier)
			{
				_tokenVerifier = tokenVerifier;
			}

			public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
			{
				var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

				if (string.IsNullOrEmpty(token))
				{
					context.Result = new UnauthorizedResult();
					return;
				}

				try
				{
					var decodedToken = await _tokenVerifier.VerifyIdTokenAsync(token);

					// Thêm thông tin user vào HttpContext
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid), // UID người dùng
					};

					// Kiểm tra và thêm các thông tin khác nếu tồn tại
					if (decodedToken.Claims.TryGetValue("email", out var email))
					{
						claims.Add(new Claim(ClaimTypes.Email, email.ToString()));
					}

					if (decodedToken.Claims.TryGetValue("name", out var name))
					{
						claims.Add(new Claim(ClaimTypes.Name, name.ToString()));
					}

					if (decodedToken.Claims.TryGetValue("picture", out var picture))
					{
						claims.Add(new Claim("picture", picture.ToString())); // Lưu URL ảnh đại diện
					}

					// Nếu bạn đã thêm custom claims vào Firebase Token, cũng có thể lấy chúng:
					if (decodedToken.Claims.TryGetValue("role", out var role))
					{
						claims.Add(new Claim(ClaimTypes.Role, role.ToString())); // Custom claim: vai trò
					}
					context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "firebase"));

					await next();
				}
				catch
				{
					context.Result = new UnauthorizedResult();
				}
			}
		}
	}

}
