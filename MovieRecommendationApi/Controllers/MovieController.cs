using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Common;
using MovieRecommendationApi.Data;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Models;
using MovieRecommendationApi.Requests;
using System.Net;
using System.Text.Json;

namespace MovieRecommendationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public MovieController(AppDbContext dbContext, IMapper mapper)
        {
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetLatestTrailers([FromQuery] PaginationRequest request)
        {
            var latestTrailers = await dbContext.Movies
                .Where(x => x.ReleaseDate != null)
                .OrderByDescending(x => x.ReleaseDate)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages)
                .ToListAsync();

            var latestTrailersDto = mapper.Map<List<MovieDto>>(latestTrailers);

            return Ok(latestTrailersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await dbContext.Movies
                .Where(x => x.Id == id)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                var error = ErrorResponse.Create("Movie not found", "movie_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var movieDto = mapper.Map<MovieDto>(movie);
            return Ok(movieDto);
        }

        [HttpGet("/search/movie")]
        public async Task<IActionResult> SearchMovies([FromQuery] SearchMovieRequest request)
        {
            IQueryable<Movie> movies = dbContext.Movies;

            if (!string.IsNullOrEmpty(request.Query))
            {
                movies = movies.Where(x => x.Title != null && x.Title.ToLower().Contains(request.Query.ToLower()));
            }

            movies = movies.Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages);



            var moviesDto = mapper.Map<List<MovieDto>>(await movies.ToListAsync());

            return Ok(moviesDto);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularMovies([FromQuery] PaginationRequest request)
        {
            var movies = await dbContext.Movies
                .OrderByDescending(x => x.Popularity)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages)
                .ToListAsync();

            var moviesDto = mapper.Map<List<MovieDto>>(movies);

            return Ok(moviesDto);
        }


        [HttpPost("{id}/rate")]
        public async Task<IActionResult> AddMovieRating(int id, [FromBody] RateMovieRequest request)
        {
            var userId = User.GetUserId();

            var movie = await dbContext.Movies
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                var error = ErrorResponse.Create("Movie not found", "movie_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var ratingList = new RatingList
            {
                MovieId = id,
                Rating = request.Rating,
                UserId = userId
            };

            dbContext.RatingLists.Add(ratingList);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("watch-list")]
        public async Task<IActionResult> AddMovieToWatchList([FromBody] AddToWatchListRequest request)
        {
            var userId = User.GetUserId();

            var movie = await dbContext.Movies
                .Where(x => x.Id == request.MovieId)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                var error = ErrorResponse.Create("Movie not found", "movie_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var watchMovie = new WatchMovie
            {
                MovieId = request.MovieId,
                UserId = userId
            };

            dbContext.WatchMovies.Add(watchMovie);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

		[HttpGet("top-trending-movies")]
        public async Task<IActionResult> GetTopTrendingMovie()
        {
            try
            {
				var res = await dbContext.Movies.OrderByDescending(x => x.Popularity).Take(10).ToListAsync();
				return Ok(res);
			}
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

		[HttpGet("watch-list")]
		public async Task<IActionResult> GetWatchList()
		{
			try
			{
				var userId = User.GetUserId();
                var res = await dbContext.Users
                    .Where(x => x.Id == userId)
                    .Include(x => x.WatchMovies)
                    .ThenInclude(a => a.Movie)
                    .ToListAsync();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpGet("rating_list")]
        public async Task<IActionResult> GetRatingList()
        {
            try
            {
                var userId = User.GetUserId();
                var res = await dbContext.Users
                    .Where(x => x.Id == userId)
                    .Include(x => x.RatingLists)
                    .ThenInclude(rl => rl.Rating)
                    .ToListAsync();
                return Ok(res);
            }
            catch(Exception ex )
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recommentdation-movie")]
        public async Task<IActionResult> GetRecommendationMovie()
        {
            try
            {
				var userId = User.GetUserId();
                var res = await dbContext.Users
                return Ok(res);
			}
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
	}
}
