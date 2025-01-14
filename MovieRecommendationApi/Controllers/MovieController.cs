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
            var latestTrailers = dbContext.Movies
                .Where(x => x.ReleaseDate != null);

            var count = await latestTrailers.CountAsync();

            var paginatedData = latestTrailers
                .OrderByDescending(x => x.ReleaseDate)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var latestTrailersDto = mapper.Map<List<MovieDto>>(paginatedData);

            var paginatedResponse = new PaginatedResponse<MovieDto>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
                Data = latestTrailersDto,
                TotalResults = count
            };


            return Ok(paginatedResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
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

            if (request.ReleaseYearFrom != null && request.ReleaseYearTo != null)
            {
                movies = movies.Where(x => x.ReleaseDate != null &&
                    x.ReleaseDate.Value.Year >= request.ReleaseYearFrom
                    && x.ReleaseDate.Value.Year <= request.ReleaseYearTo);
            }
            else if (request.ReleaseYearFrom != null)
            {
                movies = movies.Where(x => x.ReleaseDate != null &&
                    x.ReleaseDate.Value.Year >= request.ReleaseYearFrom);
            }
            else if (request.ReleaseYearTo != null)
            {
                movies = movies.Where(x => x.ReleaseDate != null &&
                    x.ReleaseDate.Value.Year <= request.ReleaseYearTo);
            }

            var count = await movies.CountAsync();

            movies = movies.Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages);



            var data = await movies.ToListAsync();

            var moviesDto = mapper.Map<List<MovieDto>>(data);

            var result = new PaginatedResponse<MovieDto>()
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
                Data = moviesDto,
                TotalResults = count
            };



            return Ok(result);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularMovies([FromQuery] PaginationRequest request)
        {
            var movies = dbContext.Movies
                .OrderByDescending(x => x.Popularity);

            var count = await movies.CountAsync();

            var paginatedResult = await movies.Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.BelongsToCollection)
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompanies)
                .Include(x => x.ProductionCountries)
                .Include(x => x.SpokenLanguages)
                .ToListAsync();

            var moviesDto = mapper.Map<List<MovieDto>>(paginatedResult);

            var response = new PaginatedResponse<MovieDto>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
                Data = moviesDto,
                TotalResults = count
            };

            return Ok(response);
        }

        [HttpPost("{id}/reviews")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> AddMovieReview(string id, [FromBody] AddReviewRequest request)
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

            var user = await dbContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();


            if (user == null)
            {
                user = new User()
                {
                    Id = userId,
                    Email = request.Username,
                    Name = request.Username,
                    AvatarPath = request.AvatarPath
                };
            }

            var review = new Review
            {
                MovieId = id,
                User = user,
                Rating = request.Rating,
                Content = request.Content
            };

            dbContext.Reviews.Add(review);

            await dbContext.SaveChangesAsync();

            return Ok(mapper.Map<ReviewDto>(review));
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
        public async Task<IActionResult> GetTopTrendingMovie([FromQuery] string type = "day")
        {
            var today = new DateTime(2024, 11, 25).ToUniversalTime();
            if (type == "day")
            {
                // Lấy ngày hiện tại
                var todayMovies = await dbContext.Movies
                .Where(movie => movie.ReleaseDate != null && movie.ReleaseDate.Value.Date == today.Date)
                .OrderByDescending(movie => movie.ReleaseDate)
                .Take(20)
                .ToListAsync();

                var movieDtos = mapper.Map<List<MovieDto>>(todayMovies);

                return Ok(movieDtos);
            }
            else
            {
                var sevenDaysAgo = today.AddDays(-7);
                var trendingMovies = await dbContext.Movies
                    .Where(movie => movie.ReleaseDate != null &&
                    movie.ReleaseDate.Value >= sevenDaysAgo && movie.ReleaseDate.Value <= today)
                    .OrderByDescending(movie => movie.ReleaseDate) // Sắp xếp theo ngày phát hành mới nhất
                    .Take(20) // Lấy tối đa 20 bộ phim
                    .ToListAsync();

                var movieDtos = mapper.Map<List<MovieDto>>(trendingMovies);

                return Ok(movieDtos);
            }
        }

        [HttpGet("watch-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> GetWatchList([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Query = null)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrWhiteSpace(Query))
            {
                var total = await dbContext.WatchMovies
                    .Where(x => x.Id == userId).ToListAsync();
                var res = total
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(x => x.Movie);

                var response = new PaginatedResponse<MovieDto>
                {
                    Page = Page,
                    PageSize = PageSize,
                    TotalPages = (int)Math.Ceiling((double)total.Count / PageSize),
                    TotalResults = total.Count,
                    Data = mapper.Map<List<MovieDto>>(res)
                };

                return Ok(response);
            }
            var total1 = await dbContext.WatchMovies
                .Where(x => x.Id == userId && x.Movie.Title != null && x.Movie.Title.Contains(Query)).ToListAsync();
            var res1 = total1
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .Select(x => x.Movie);

            var response1 = new PaginatedResponse<MovieDto>
            {
                Page = Page,
                PageSize = PageSize,
                TotalPages = (int)Math.Ceiling((double)total1.Count / PageSize),
                TotalResults = total1.Count,
                Data = mapper.Map<List<MovieDto>>(res1)
            };

            return Ok(response1);
        }


        [HttpGet("rating-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> GetRatingList([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Query = null)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrWhiteSpace(Query))
            {
                var totalRes = await dbContext.Reviews
                .Where(x => x.UserId == userId).ToListAsync();

                var res = totalRes
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .Select(rl => new
                {
                    rating = rl.Rating,
                    movie = rl.Movie,
                });
                return Ok(new
                {
                    data = res,
                    totalPages = (int)Math.Ceiling((double)totalRes.Count / PageSize)
                });

            }
            var totalRes1 = await dbContext.Reviews
                .Where(x => x.UserId == userId && x.Movie.Title.Contains(Query)).ToListAsync();
            var res1 = totalRes1
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .Select(rl => new
                {
                    rating = rl.Rating,
                    movie = rl.Movie,
                });

            return Ok(new
            {
                data = res1,
                totalPages = (int)Math.Ceiling((double)totalRes1.Count / PageSize)
            });
        }

        [HttpGet("recommentdation-movie")]
        public async Task<IActionResult> GetRecommendationMovie([FromQuery] string? id)
        {
            var movie = await dbContext.Movies
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                var error = ErrorResponse.Create("Movie not found", "movie_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }


            var response = await dbContext.SimilarMovies
                .Where(x => x.MovieId == id)
                .Include(x => x.SimilarMovieEntity)
                .Select(x => x.SimilarMovieEntity)
                .ToListAsync();


            return Ok(mapper.Map<List<MovieDto>>(response));
        }


        [HttpGet("{id}/movie-review")]
        public async Task<IActionResult> GetMovieReview(string id, [FromQuery] int Page = 1, [FromQuery] int PageSize = 20)
        {
            var movie = await dbContext.Movies
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                var error = ErrorResponse.Create("Movie not found", "movie_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var totalRes = await dbContext.Reviews
                .Where(x => x.MovieId == id)
                .Include(x => x.User)
                .ToListAsync();

            var res = totalRes.Skip(PageSize * (Page - 1)).Take(PageSize);

            var response = new PaginatedResponse<ReviewDto>
            {
                Page = Page,
                PageSize = PageSize,
                TotalPages = (int)Math.Ceiling((double)totalRes.Count / PageSize),
                TotalResults = totalRes.Count,
                Data = mapper.Map<List<ReviewDto>>(res)
            };

            return Ok(response);
        }

        [HttpGet("{id}/video-movie")]
        public async Task<IActionResult> GetVideoMovie(string id, [FromQuery] int Page = 1, [FromQuery] int PageSize = 20)
        {
            var movie = await dbContext.Movies
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                var error = ErrorResponse.Create("Movie not found", "movie_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var totalRes = await dbContext.Videos.Where(x => x.MovieId == id).ToListAsync();

            var res = totalRes.Skip(PageSize * (Page - 1)).Take(PageSize);

            var response = new PaginatedResponse<VideoDto>
            {
                Page = Page,
                PageSize = PageSize,
                TotalPages = (int)Math.Ceiling((double)totalRes.Count / PageSize),
                TotalResults = totalRes.Count,
                Data = mapper.Map<List<VideoDto>>(res)
            };

            return Ok(response);
        }


        [HttpGet("favorite-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> GetFavoriteList([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Query = null)
        {
            var userId = User.GetUserId();


            var favoriteMovies = dbContext.FavoriteMovies.Where(f => f.UserId == userId);

            if (!string.IsNullOrEmpty(Query))
            {
                favoriteMovies = favoriteMovies.Where(f => f.Movie.Title.Contains(Query));
            }

            var total = await dbContext.FavoriteMovies.CountAsync();

            var res = favoriteMovies
                   .Skip((Page - 1) * PageSize)
                   .Take(PageSize)
                   .Select(x => x.Movie).ToListAsync();

            var response = new PaginatedResponse<MovieDto>
            {
                Page = Page,
                PageSize = PageSize,
                TotalPages = (int)Math.Ceiling((double)total / PageSize),
                TotalResults = total,
                Data = mapper.Map<List<MovieDto>>(res)
            };

            return Ok(response);
        }


        [HttpPost("favorite-list")]
        [FirebaseAuthenticationAttribute]
        public async Task<IActionResult> AddFavoriteList([FromBody] AddFavoriteVM model)
        {
            var userId = User.GetUserId();
            List<FavoriteMovie> add = new List<FavoriteMovie>();
            foreach (var item in model.Movies)
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

        [HttpGet("list-genre-by-id")]
        public async Task<IActionResult> GetGenres([FromQuery] List<string> genre_ids)
        {
            if (genre_ids == null || !genre_ids.Any())
            {
                return BadRequest("No genre_ids provided.");
            }

            var genres = await dbContext.Genres
                .Where(g => genre_ids.Contains(g.Id))
                .ToListAsync();

            return Ok(mapper.Map<List<GenreDto>>(genres));
        }

        [HttpGet("list-movie-by-id")]
        public async Task<IActionResult> GetMovies([FromQuery] List<string> movie_ids)
        {
            if (movie_ids == null || !movie_ids.Any())
            {
                return BadRequest("No genre_ids provided.");
            }

            var movies = await dbContext.Movies
                .Where(g => movie_ids.Contains(g.Id))
                .ToListAsync();

            return Ok(mapper.Map<List<MovieDto>>(movies));
        }

        [HttpGet("list-cast-by-id-movie")]
        public async Task<IActionResult> GetCastByMovieId([FromQuery] string id)
        {
            var movie = await dbContext.Movies
                 .Where(g => g.Id == id)
                 .Include(g => g.Credit)
                 .ThenInclude(c => c.Cast)
                 .FirstOrDefaultAsync();

            if (movie == null)
            {
                var error = ErrorResponse.Create("Movie not found", "movie_not_found", HttpStatusCode.NotFound);
                return error.MapErrorResponse();
            }

            var casts = movie.Credit?.Cast ?? [];

            var response = mapper.Map<List<PersonDto>>(casts);

            return Ok(response);
        }
    }

    public class AddFavoriteVM
    {
        public List<string>? Movies { set; get; }
    }
}
