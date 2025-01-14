using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Common;
using MovieRecommendationApi.Data;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Models;
using System.Net;
using System.Text.Json;

namespace MovieRecommendationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public PersonController(AppDbContext dbContext, IMapper mapper)
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
        public async Task<IActionResult> GetAll()
        {
            var people = await dbContext.People.ToListAsync();


            return Ok(mapper.Map<List<PersonDto>>(people));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var person = await dbContext.People
                .Where(x => x.Id == id)
                .Include(x => x.MovieCredit)
                .FirstOrDefaultAsync();

            if (person == null)
            {
                var error = ErrorResponse.Create("Person not found", "person_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var personDto = mapper.Map<PersonDto>(person);

            return Ok(personDto);
        }

        [HttpGet("{id}/movie_credits")]
        public async Task<IActionResult> GetActingList(string id)
        {
            var person = await dbContext.People
                .Where(x => x.Id == id)
                .Include(x => x.MovieCredit)
                .FirstOrDefaultAsync();

            if (person == null)
            {
                var error = ErrorResponse.Create("Person not found", "person_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var movieCredits = person.MovieCredit;

            if (movieCredits == null)
            {
                var error = ErrorResponse.Create("Movie credits not found", "movie_credits_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var movieCreditsDto = mapper.Map<MovieCredit>(movieCredits);


            return Ok(movieCreditsDto);

        }


    }
}
