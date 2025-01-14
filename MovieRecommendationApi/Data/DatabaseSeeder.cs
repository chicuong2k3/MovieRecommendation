using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Data
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            var jsonData = File.ReadAllText("tmdb_db.movies.json");
            var movies = JsonSerializer.Deserialize<List<Movie>>(jsonData);

            if (movies == null || !movies.Any()) return;

            //SeedGenres(context);

            //SeedBelongsToCollections(context, movies);
            //SeedProductionCompanies(context, movies);
            //SeedProductionCountries(context, movies);
            //SeedSpokenLanguages(context, movies);

            //SeedVideos(context, movies);

            //SeedPeople(context);

            // SeedCredits(context, movies);
            //SeedMovies(context, movies);

            //SeedMovieCredits(context);

            //SeedSimilarMovies(context);

            //SeedReviews(context, movies);

            Console.WriteLine("Database seeding complete.");
        }

        private static void SeedGenres(AppDbContext context)
        {
            var jsonData = File.ReadAllText("tmdb_db.movie_genres.json");
            var genres = JsonSerializer.Deserialize<List<Genre>>(jsonData);

            if (genres == null || !genres.Any()) return;

            foreach (var genre in genres)
            {
                if (!context.Genres.Any(g => g.Id == genre.Id))
                {
                    context.Genres.Add(genre);
                }
            }

            context.SaveChanges();
        }

        private static void SeedBelongsToCollections(AppDbContext context, List<Movie> movies)
        {
            var collections = movies
                .Where(m => m.BelongsToCollection != null)
                .Select(m => m.BelongsToCollection).Take(500)
                .DistinctBy(c => c.Id)
                .ToList();

            foreach (var collection in collections)
            {
                if (!context.BelongsToCollections.Any(c => c.Id == collection!.Id))
                {
                    context.BelongsToCollections.Add(collection!);
                }
            }

            context.SaveChanges();
        }

        private static void SeedProductionCompanies(AppDbContext context, List<Movie> movies)
        {
            var companies = movies
                .SelectMany(m => m.ProductionCompanies ?? new List<ProductionCompany>())
                .DistinctBy(c => c.Id).Take(500)
                .ToList();

            foreach (var company in companies)
            {
                if (!context.ProductionCompanies.Any(c => c.Id == company.Id))
                {
                    context.ProductionCompanies.Add(company);
                }
            }

            context.SaveChanges();
        }

        private static void SeedProductionCountries(AppDbContext context, List<Movie> movies)
        {
            var countries = movies
                .SelectMany(m => m.ProductionCountries ?? new List<ProductionCountry>())
                .DistinctBy(c => c.Id)
                .ToList();

            foreach (var country in countries)
            {
                if (!context.ProductionCountries.Any(c => c.Id == country.Id))
                {
                    context.ProductionCountries.Add(country);
                }
            }

            context.SaveChanges();
        }


        private static void SeedSpokenLanguages(AppDbContext context, List<Movie> movies)
        {
            var languages = movies
                .SelectMany(m => m.SpokenLanguages ?? new List<SpokenLanguage>())
                .DistinctBy(p => p.Id)
                .ToList();

            foreach (var language in languages)
            {
                if (!context.SpokenLanguages.Any(p => p.Id == language.Id))
                {
                    context.SpokenLanguages.Add(language);
                }
            }

            context.SaveChanges();
        }
        private static void SeedCredits(AppDbContext context, List<Movie> movies)
        {
            // Extract unique credits from the movies list
            var credits = movies
                .Select(m => m.Credits)
                .Where(c => c != null)
                .DistinctBy(c => c.Id)
                .Take(200)
                .ToList();

            // Fetch all existing People IDs into a HashSet for fast lookups
            var existingPeople = context.People;

            foreach (var credit in credits)
            {
                // Check if the credit already exists
                if (!context.Credits.Any(c => c.Id == credit.Id))
                {
                    if (credit.Cast != null)
                    {
                        var castIds = credit.Cast.Select(c => c.IdForCrawling);
                        credit.Cast = existingPeople.Where(x => castIds.Contains(x.IdForCrawling)).ToList();
                    }

                    // Add the credit
                    context.Credits.Add(credit);


                    context.SaveChanges();
                }
            }


        }




        private static void SeedVideos(AppDbContext context, List<Movie> movies)
        {
            var videos = movies
                .SelectMany(m => m.Videos ?? new List<Video>())
                .DistinctBy(v => v.Id).Take(500)
                .ToList();

            foreach (var video in videos)
            {
                if (!context.Videos.Any(v => v.Id == video.Id))
                {
                    context.Videos.Add(video);
                }
            }

            context.SaveChanges();
        }

        private static void SeedMovies(AppDbContext context, List<Movie> movies)
        {
            var i = 0;

            // Retrieve related data for efficiency
            var genres = context.Genres.ToList();
            var collections = context.BelongsToCollections.ToList();
            var productCompanies = context.ProductionCompanies.ToList();
            var productCountries = context.ProductionCountries.ToList();
            var spokenLanguages = context.SpokenLanguages.ToList();
            var credits = context.Credits.ToList();
            var trailers = context.Videos.ToList();

            foreach (var movie in movies)
            {
                // Check if the movie already exists in the database
                if (!context.Movies.Any(m => m.Id == movie.Id))
                {
                    // Get related genre IDs
                    var genreIds = movie.Genres?.Select(x => x.IdForCrawling).ToList();

                    // Handle other relationships similarly if needed, e.g. collections, production companies, etc.
                    var collectionId = movie.BelongsToCollection?.Id;
                    var countryIds = movie.ProductionCountries?.Select(x => x.Id).ToList();
                    var companiesIds = movie.ProductionCompanies?.Select(x => x.Id).ToList();
                    var languageIds = movie.SpokenLanguages?.Select(x => x.Id).ToList();
                    var creditId = movie.Credits?.Id;
                    var trailerIds = movie.Videos?.Select(x => x.Id).ToList();

                    movie.Genres = genres.Where(x => genreIds.Contains(x.IdForCrawling)).ToList();
                    movie.BelongsToCollection = collections.Where(x => collectionId == x.Id).FirstOrDefault();
                    movie.ProductionCompanies = productCompanies.Where(x => companiesIds != null && companiesIds.Contains(x.Id)).ToList();
                    movie.ProductionCountries = productCountries.Where(x => countryIds != null && countryIds.Contains(x.Id)).ToList();
                    movie.SpokenLanguages = spokenLanguages.Where(x => languageIds != null && languageIds.Contains(x.Id)).ToList();
                    movie.Credits = credits.Where(x => creditId == x.Id).FirstOrDefault();
                    movie.Videos = trailers.Where(x => trailerIds != null && trailerIds.Contains(x.Id)).ToList();

                    // Add the movie and related entities
                    context.Movies.Add(movie);
                }

                if (i >= 1000)
                {
                    break;
                }

                i++;
            }

            // Save the changes to the database
            context.SaveChanges();
        }


        private static void SeedPeople(AppDbContext context)
        {
            // Đọc dữ liệu JSON từ file
            var jsonData = File.ReadAllText("tmdb_db.people.json");
            var people = JsonSerializer.Deserialize<List<Person>>(jsonData);

            if (people == null || !people.Any()) return;

            foreach (var person in people)
            {
                var existingPerson = context.People
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Id == person.Id);
                if (existingPerson != null)
                {
                    existingPerson.Adult = person.Adult;
                    existingPerson.ImdbId = person.ImdbId;
                    existingPerson.TmdbId = person.TmdbId;
                    existingPerson.AlsoKnownAs = person.AlsoKnownAs;
                    existingPerson.Biography = person.Biography;
                    existingPerson.Birthday = person.Birthday;
                    existingPerson.Deathday = person.Deathday;
                    existingPerson.Gender = person.Gender;
                    existingPerson.Homepage = person.Homepage;
                    existingPerson.KnownForDepartment = person.KnownForDepartment;
                    existingPerson.Name = person.Name;
                    existingPerson.PlaceOfBirth = person.PlaceOfBirth;
                    existingPerson.Popularity = person.Popularity;
                    existingPerson.ProfilePath = person.ProfilePath;

                }
                else
                {
                    person.MovieCredit = null;
                    context.People.Add(person);
                }

                if (context.ChangeTracker.Entries().Count() > 500)
                {
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
            }

            context.SaveChanges();
        }


        private static void SeedMovieCredits(AppDbContext context)
        {
            var jsonData = File.ReadAllText("tmdb_db.people.json");
            var people = JsonSerializer.Deserialize<List<Person>>(jsonData);

            if (people == null || !people.Any()) return;


            // Extract unique credits from the movies list
            var movieCredits = people
                .Select(m => m.MovieCredit)
                .Where(c => c != null)
                .Take(500)
                .ToList();

            var existingMovie = context.Movies;

            foreach (var movieCredit in movieCredits)
            {
                // Check if the credit already exists
                if (!context.MovieCredits.Any(c => c.Id == movieCredit.Id))
                {
                    if (movieCredit.Cast != null)
                    {
                        var castTitles = movieCredit.Cast.Select(c => c.Title.ToLower());
                        movieCredit.Cast = existingMovie.Where(x => castTitles.Contains(x.Title.ToLower())).ToList();
                    }

                    // Add the credit
                    context.MovieCredits.Add(movieCredit);


                    context.SaveChanges();
                }
            }


        }


        private static void SeedSimilarMovies(AppDbContext context)
        {
            if (context.SimilarMovies.Any()) return;

            var jsonData = File.ReadAllText("tmdb_db.similar.json");
            var similarMoviesData = JsonSerializer.Deserialize<List<SimilarMovieModel>>(jsonData);

            if (similarMoviesData == null || !similarMoviesData.Any()) return;

            foreach (var item in similarMoviesData)
            {
                var movie = context.Movies.FirstOrDefault(m => m.TmdbId == item.TmdbId);
                if (movie == null) continue;

                foreach (var similarMovie in item.SimilarMovies)
                {
                    var similarMovieEntity = context.Movies.FirstOrDefault(m => m.Title.ToLower() == similarMovie.Title.ToLower());
                    if (similarMovieEntity == null) continue;

                    var similarMovieRelation = new SimilarMovie
                    {
                        MovieId = movie.Id,
                        SimilarMovieId = similarMovieEntity.Id
                    };

                    context.SimilarMovies.Add(similarMovieRelation);

                    if (context.ChangeTracker.Entries().Count() > 100)
                    {
                        context.SaveChanges();
                        context.ChangeTracker.Clear();
                    }
                }
            }

            context.SaveChanges();
        }


        private static void SeedReviews(AppDbContext context, List<Movie> movies)
        {
            var reviews = movies
                .Take(500)
                .SelectMany(movie => movie.ReviewModels?.Select(review => (movie.Id, review))
                             ?? Enumerable.Empty<(string MovieId, ReviewModel ReviewModel)>())
                .ToList();

            foreach (var reviewData in reviews)
            {
                var reviewModel = reviewData.Item2;
                var movieId = reviewData.Item1;

                if (reviewModel == null)
                {
                    continue;
                }

                var user = context.Users
                    .Where(u => reviewModel.AuthorDetails != null && u.Email == reviewModel.AuthorDetails.UserName)
                    .FirstOrDefault();

                if (user == null)
                {
                    user = new User
                    {
                        Email = reviewModel.AuthorDetails?.UserName,
                        AvatarPath = reviewModel.AuthorDetails?.AvatarPath,
                        Name = reviewModel.AuthorDetails?.Name ?? reviewModel.Author
                    };

                    context.Users.Add(user);
                    context.SaveChanges();
                }

                var review = new Review
                {
                    Id = reviewModel.Id,
                    //User = user,
                    MovieId = movieId, // Assign the MovieId here
                    Content = reviewModel.Content,
                    CreatedAt = reviewModel.CreatedAt,
                    UpdatedAt = reviewModel.UpdatedAt,
                    Url = reviewModel.Url,
                    Rating = reviewModel.AuthorDetails?.Rating ?? 5
                };

                context.Reviews.Add(review); // Add the review to the context
            }

            context.SaveChanges(); // Save all changes in one batch
        }

    }

    public class ReviewModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }
        [JsonPropertyName("author_details")]
        public AuthorDetails? AuthorDetails { get; set; }
    }

    public class AuthorDetails
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("username")]
        public string? UserName { get; set; }
        [JsonPropertyName("avatar_path")]
        public string? AvatarPath { get; set; }
        [JsonPropertyName("rating")]
        [JsonConverter(typeof(NullableIntConverter))]
        public int? Rating { get; set; }
    }
}
