
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace MovieRecommendationApi.Controllers.AuthenFirebase
{
	public class FirebaseTokenVerifier
	{
		public FirebaseTokenVerifier()
		{
			// Khởi tạo Firebase Admin SDK
			FirebaseApp.Create(new AppOptions
			{
				Credential = GoogleCredential.FromFile("final-awad-firebase-adminsdk-gyhou-f59ac6756c.json")
			});
		}

		public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
		{
			try
			{
				FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
				return decodedToken; // Trả về thông tin user nếu token hợp lệ
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Token verification failed: {ex.Message}");
				throw new UnauthorizedAccessException("Invalid ID token.");
			}
		}
	}
}
