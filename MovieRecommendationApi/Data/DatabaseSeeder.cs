using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Models;
using System.Text.Json;

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

            //SeedGenres(context, movies);
            //SeedBelongsToCollections(context, movies);
            //SeedProductionCompanies(context, movies);
            //SeedProductionCountries(context, movies);
            //SeedSpokenLanguages(context, movies);

            //SeedVideos(context, movies);

            //SeedPeople(context);

            //SeedCredits(context, movies);
            //SeedMovies(context, movies);
            //SeedSimilarMovies(context);

            Console.WriteLine("Database seeding complete.");
        }

        private static void SeedGenres(AppDbContext context, List<Movie> movies)
        {
            var genres = movies
                .SelectMany(m => m.Genres ?? new List<Genre>())
                .DistinctBy(g => g.Id)
                .ToList();

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
                .Select(m => m.BelongsToCollection)
                .DistinctBy(c => c.Id)
                .ToList();

            foreach (var collection in collections)
            {
                if (!context.BelongsToCollections.Any(c => c.Id == collection.Id))
                {
                    context.BelongsToCollections.Add(collection);
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
                .Take(100)
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
                        var castIds = credit.Cast.Select(c => c.Id);
                        credit.Cast = existingPeople.Where(x => castIds.Contains(x.Id)).ToList();
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
                .SelectMany(m => m.Trailers ?? new List<Video>())
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
                    var genreIds = movie.Genres?.Select(x => x.Id).ToList() ?? new List<int>();

                    // Handle other relationships similarly if needed, e.g. collections, production companies, etc.
                    var collectionId = movie.BelongsToCollection?.Id;
                    var countryIds = movie.ProductionCountries?.Select(x => x.Id).ToList();
                    var companiesIds = movie.ProductionCompanies?.Select(x => x.Id).ToList();
                    var languageIds = movie.SpokenLanguages?.Select(x => x.Id).ToList();
                    var creditId = movie.Credits?.Id;
                    var trailerIds = movie.Trailers?.Select(x => x.Id).ToList();

                    movie.Genres = genres.Where(x => genreIds.Contains(x.Id)).ToList();
                    movie.BelongsToCollection = collections.Where(x => collectionId == x.Id).FirstOrDefault();
                    movie.ProductionCompanies = productCompanies.Where(x => companiesIds != null && companiesIds.Contains(x.Id)).ToList();
                    movie.ProductionCountries = productCountries.Where(x => countryIds != null && countryIds.Contains(x.Id)).ToList();
                    movie.SpokenLanguages = spokenLanguages.Where(x => languageIds != null && languageIds.Contains(x.Id)).ToList();
                    movie.Credits = credits.Where(x => creditId == x.Id).FirstOrDefault();
                    movie.Trailers = trailers.Where(x => trailerIds != null && trailerIds.Contains(x.Id)).ToList();

                    // Add the movie and related entities
                    context.Movies.Add(movie);
                }

                if (i >= 200)
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

                    if (person.MovieCredits != null)
                    {
                        existingPerson.MovieCredits = person.MovieCredits;
                    }
                }
                else
                {
                    context.People.Add(person);
                }

                if (context.ChangeTracker.Entries().Count() > 100)
                {
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
            }

            context.SaveChanges();
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
                    var similarMovieEntity = context.Movies.FirstOrDefault(m => m.Id == similarMovie.Id);
                    if (similarMovieEntity == null) continue;

                    var similarMovieRelation = new SimilarMovie
                    {
                        MovieId = movie.Id,
                        SimilarMovieId = similarMovie.Id
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

    }
}
