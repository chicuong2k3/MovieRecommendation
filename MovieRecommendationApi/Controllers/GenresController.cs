using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Data;
using MovieRecommendationApi.Dtos;
using System.Text.Json;

namespace MovieRecommendationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public GenresController(AppDbContext dbContext, IMapper mapper)
        {
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await dbContext.Genres.ToListAsync();
            return Ok(genres.Adapt<List<GenreDto>>(mapper.Config));
        }
    }
}
