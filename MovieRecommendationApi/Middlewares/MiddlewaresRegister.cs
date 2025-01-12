using FluentValidation;

namespace MovieRecommendationApi.Middlewares
{
    public static class MiddlewaresRegister
    {
        public static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            var assemblies = new[] { typeof(MiddlewaresRegister).Assembly };
            services.AddValidatorsFromAssemblies(assemblies);

            services.AddScoped<ValidationMiddleware>();
            services.AddScoped<GlobalExceptionMiddleware>();
            services.AddScoped<RequestResponseLoggingMiddleware>();

            Console.WriteLine("Middlewares Registered");

            return services;
        }
        public static void UseMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<ValidationMiddleware>();
        }
    }
}
