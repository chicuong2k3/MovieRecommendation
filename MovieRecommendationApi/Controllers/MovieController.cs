using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Common;
using MovieRecommendationApi.Controllers.AuthenFirebase;
using MovieRecommendationApi.Data;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Models;
using MovieRecommendationApi.Requests;
using System.Net;
using System.Reflection.Metadata;
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
            IQueryable<Movie> movies = dbContext.Movies.Include(x => x.Genres);

            if (!string.IsNullOrEmpty(request.Query))
            {
                movies = movies.Where(x => x.Title != null && x.Title.ToLower().Contains(request.Query.ToLower()));
            }

            if (request.GenreIds != null && request.GenreIds.Any())
            {
                movies = movies.Where(x => x.Genres != null &&
                    x.Genres.Any(g => request.GenreIds.Contains(g.Id)));
            }

            if (request.Ratings != null && request.Ratings.Any())
            {
                var movieIds = await dbContext.RatingLists
                    .Where(x => request.Ratings.Contains(x.Rating))
                    .Select(x => x.MovieId)
                    .ToListAsync();

                movies = movies.Where(x => movieIds.Contains(x.Id));
            }

            if (request.ReleaseYearFrom != null && request.ReleaseYearTo != null)
            {
                movies = movies.Where(x => x.ReleaseDate != null &&
                    x.ReleaseDate.Value.Year >= request.ReleaseYearFrom
                    && x.ReleaseDate.Value.Year <= request.ReleaseYearTo);
            }

            movies = movies.Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages);
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
        [FirebaseAuthenticationAttribute]
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
        [FirebaseAuthenticationAttribute]
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("watch-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> GetWatchList([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Query = null)
        {
            try
            {
                var userId = User.GetUserId();
                if (string.IsNullOrWhiteSpace(Query))
                {
                    var res = await dbContext.WatchMovies
                    .Where(x => x.Id == userId)
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(x => x.Movie)
                    .ToListAsync();
                    return Ok(res);
                }
                var res1 = await dbContext.WatchMovies
                    .Where(x => x.Id == userId && x.Movie.Title.Contains(Query))
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(x => x.Movie)
                    .ToListAsync();
                return Ok(res1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("rating-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> GetRatingList([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Query = null)
        {
            try
            {
                var userId = User.GetUserId();
                if (string.IsNullOrWhiteSpace(Query))
                {
                    var res = await dbContext.RatingLists
                    .Where(x => x.UserId == userId && x.Movie.Title.Contains(Query))
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(rl => new
                    {
                        rating = rl.Rating,
                        movie = rl.Movie,
                    })
                    .ToListAsync();
                    return Ok(res);

                }
                var res1 = await dbContext.RatingLists
                    .Where(x => x.UserId == userId && x.Movie.Title.Contains(Query))
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(rl => new
                    {
                        rating = rl.Rating,
                        movie = rl.Movie,
                    })
                    .ToListAsync();
                return Ok(res1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recommentdation-movie")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> GetRecommendationMovie([FromQuery] string? genre)
        {
            try
            {
                var userId = User.GetUserId();
                if (string.IsNullOrWhiteSpace(genre))
                {
                    var res = await dbContext.Movies
                        .ToListAsync();
                    return Ok(res);
                }
                var res1 = await dbContext.Movies
                         .Include(x => x.Genres)
                         .Where(x => x.Genres.Any(g => g.Name.Contains(genre)))
                         .ToListAsync();
                return Ok(res1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}/movie-review")]
        public async Task<IActionResult> GetMovieReview(int id, [FromQuery] int Page = 1, [FromQuery] int PageSize = 20)
        {
            try
            {
                var res = await dbContext.Reviews.Where(x => x.MovieId == id).
                Skip(PageSize * (Page - 1)).Take(PageSize).ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/video-movie")]
        public async Task<IActionResult> GetVideoMovie(int id, [FromQuery] int Page = 1, [FromQuery] int PageSize = 20)
        {
            try
            {
                var res = await dbContext.Videos.Where(x => x.MovieId == id).
                Skip(PageSize * (Page - 1)).Take(PageSize).ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("favorite-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> GetFavoriteList([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Query = null)
        {
            try
            {
                var userId = User.GetUserId();
                if (string.IsNullOrWhiteSpace(Query))
                {
                    var res = await dbContext.FavoriteMovies
                        .Where(f => f.UserId == userId)
                        .Skip((Page - 1) * PageSize)
                        .Take(PageSize)
                        .Select(x => x.Movie)
                        .ToListAsync();
                    return Ok(res);
                }
                var res1 = await dbContext.FavoriteMovies
                       .Where(f => f.UserId == userId && f.Movie.Title.Contains(Query))
                       .Skip((Page - 1) * PageSize)
                       .Take(PageSize)
                       .Select(x => x.Movie)
                       .ToListAsync();
                return Ok(res1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("favorite-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> AddFavoriteList([FromBody] AddFavoriteVM model)
        {
            try
            {
                var userId = User.GetUserId();
                List<FavoriteMovie> add = new List<FavoriteMovie>();
                foreach (var item in model.movies)
                {
                    add.Add(new FavoriteMovie()
                    {
                        UserId = userId,
                        MovieId = item
                    });
                }
                await dbContext.FavoriteMovies.AddRangeAsync(add);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list-genre-by-id")]
        public async Task<IActionResult> GetGenres([FromQuery] List<int> genre_ids)
        {
            if (genre_ids == null || !genre_ids.Any())
            {
                return BadRequest("No genre_ids provided.");
            }

            try
            {
                var genres = await dbContext.Genres
                .Where(g => genre_ids.Contains(g.Id))
                .ToListAsync();

                return Ok(genres);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class AddFavoriteVM
    {
        public List<int> movies { set; get; }
    }
}
