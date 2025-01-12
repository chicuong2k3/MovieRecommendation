using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using MovieRecommendationApi.Middlewares;
using System;
using System.Text;
using MovieRecommendationApi.Data;
using MovieRecommendationApi.DataShaping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IPropertyChecker, PropertyChecker>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMiddlewares();
//builder.Services.AddScoped<ClaimsTransformationMiddleware>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
});

// Đọc cấu hình JWT từ appsettings
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Cấu hình dịch vụ xác thực JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
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
        var origins = new List<string>();
        builder.Configuration.Bind("Cors:Origins", origins);
        if (origins.Any())
        {
            policyBuilder
                .WithOrigins(origins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    });
});



var app = builder.Build();

app.UseCors(builder =>
      builder.WithOrigins("http://localhost:3000")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


//app.UseMiddleware<ClaimsTransformationMiddleware>();
app.UseAuthorization();


app.MapControllers();

app.Run();