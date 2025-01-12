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

            // Ensure database is created
            context.Database.EnsureCreated();

            // Seed data
            SeedMovies(context);
            SeedPeople(context);

            Console.WriteLine("Database seeding complete.");
        }

        private static void SeedMovies(AppDbContext context)
        {
            if (context.Movies.Any()) return;

            // Read JSON data
            var jsonData = File.ReadAllText("tmdb_db.movies.json");
            var movies = JsonSerializer.Deserialize<List<Movie>>(jsonData);

            if (movies == null) return;

            // Resolve relationships and add to context
            foreach (var movie in movies)
            {
                // Ensure related entities exist
                ResolveRelationships(context, movie);

                // Add movie to database
                context.Movies.Add(movie);

                // Save in batches for performance
                if (context.ChangeTracker.Entries().Count() > 1000)
                {
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
            }

            context.SaveChanges();
        }

        private static void SeedPeople(AppDbContext context)
        {
            if (context.People.Any()) return;

            // Read JSON data
            var jsonData = File.ReadAllText("tmdb_db.people.json");
            var people = JsonSerializer.Deserialize<List<Person>>(jsonData);

            if (people == null) return;

            // Add people to database
            foreach (var person in people)
            {
                // Ensure credits are resolved
                ResolveCredits(context, person);

                context.People.Add(person);

                // Save in batches for performance
                if (context.ChangeTracker.Entries().Count() > 1000)
                {
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
            }

            context.SaveChanges();
        }

        private static void ResolveRelationships(AppDbContext context, Movie movie)
        {
            // Resolve Genres
            if (movie.Genres != null)
            {
                movie.Genres = movie.Genres
                    .Select(genre =>
                        context.Genres
                            .FirstOrDefault(g => g.Id == genre.Id) ?? genre)
                    .ToList();
            }

            // Resolve Production Companies
            if (movie.ProductionCompanies != null)
            {
                movie.ProductionCompanies = movie.ProductionCompanies
                    .Select(company =>
                        context.ProductionCompanies
                            .FirstOrDefault(pc => pc.Id == company.Id) ?? company)
                    .ToList();
            }

            // Resolve BelongsToCollection
            if (movie.BelongsToCollection != null)
            {
                var existingCollection = context.BelongsToCollections
                    .FirstOrDefault(c => c.Id == movie.BelongsToCollection.Id);

                if (existingCollection != null)
                {
                    movie.BelongsToCollection = existingCollection;
                }
            }
        }

        private static void ResolveCredits(AppDbContext context, Person person)
        {

        }
    }
}
