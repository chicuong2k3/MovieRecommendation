using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieRecommendationApi.Middlewares;
using MovieRecommendationApi.Data;
using MovieRecommendationApi.DataShaping;
using Mapster;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Controllers.AuthenFirebase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMapster();
TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterProfile).Assembly);

builder.Services.AddTransient<IPropertyChecker, PropertyChecker>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMiddlewares();
// Thêm dịch vụ FirebaseTokenVerifier vào DI
builder.Services.AddSingleton<FirebaseTokenVerifier>();
//builder.Services.AddScoped<ClaimsTransformationMiddleware>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend ANHCUONGDUY", Version = "v1" });

    // Swagger 2.+ support
    //                var security = new Dictionary<string, IEnumerable<string>>
    //                {
    //                    {"Bearer", new string[] { }},
    //                };
    //                
    //                c.AddSecurityDefinition("Bearer",
    //                    new OpenApiSecurityScheme()
    //                    {
    //                        In = ParameterLocation.Header,
    //                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
    //                      Enter 'Bearer' [space] and then your token in the text input below.
    //                      \r\n\r\nExample: 'Bearer 12345abcdef'",
    //                        Name = "Authorization", 
    //                        Type = SecuritySchemeType.ApiKey 
    //                    });
    c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
    {
        Description = "Api key needed to access the endpoints. Authorization: Bearer xxxx",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Authorization"
                            },
                        },
                        new string[] {}
                    }
            });
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});



var app = builder.Build();

//app.Services.SeedDatabase();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseMiddleware<FirebaseAuthenticationMiddleware>();

app.UseHttpsRedirection();


//app.UseMiddleware<ClaimsTransformationMiddleware>();
app.UseAuthorization();


app.MapControllers();

app.Run();