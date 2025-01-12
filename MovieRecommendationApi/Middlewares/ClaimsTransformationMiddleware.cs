//using Microsoft.EntityFrameworkCore;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace BookStack.WebApp.Api.Middlewares;

//internal class ClaimsTransformationMiddleware : IMiddleware
//{
//    private readonly AppDbContext _dbContext;

//    public ClaimsTransformationMiddleware(AppDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//    {
//        var user = context.User;

//        if (user.Identity != null)
//        {
//            var userId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

//            if (!string.IsNullOrEmpty(userId))
//            {
//                var roles = await _dbContext.Roles.Where(r => r.UserId == userId).ToListAsync();

//                if (!roles.Any())
//                {
//                    var email = user.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
//                    if (email == "nguyenchicuongk21@gmail.com")
//                    {
//                        _dbContext.Roles.Add(new Role { UserId = userId, Name = "admin" });
//                    }
//                    else
//                    {
//                        _dbContext.Roles.Add(new Role { UserId = userId, Name = "normal" });
//                    }

//                    await _dbContext.SaveChangesAsync();
//                }

//                var rolesClaim = string.Join(",", roles.Select(r => r.Name));
//                var identity = (ClaimsIdentity)user.Identity;
//                identity.AddClaim(new Claim("roles", rolesClaim));
//            }
//        }

//        await next(context);
//    }
//}