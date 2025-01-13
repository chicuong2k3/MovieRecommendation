using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Models;
using System.Reflection.Emit;

namespace MovieRecommendationApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // table moviecast
            builder.Entity<MovieCast>()
                .HasKey(x => new { x.CastId, x.MovieId });

            builder.Entity<MovieCast>()
                .HasOne(x => x.Cast)
                .WithMany(x => x.MovieCasts)
                .HasForeignKey(x => x.CastId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MovieCast>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieCasts)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            //table Rating List
            builder.Entity<RatingList>()
                .HasKey(x => new { x.MovieId, x.UserId });
            builder.Entity<RatingList>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.RatingLists)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RatingList>()
                   .HasOne(x => x.User)
                   .WithMany(x => x.RatingLists)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
            // table Review
            builder.Entity<Review>()
                .HasKey(x => new { x.MovieId, x.UserId });
            builder.Entity<Review>()
                .HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Review>()
               .HasOne(x => x.Movie)
               .WithMany(x => x.Reviews)
               .HasForeignKey(x => x.MovieId)
               .OnDelete(DeleteBehavior.NoAction);

            //table WatchMovie
            builder.Entity<WatchMovie>()
                .HasKey(x => new { x.UserId, x.MovieId });
            builder.Entity<WatchMovie>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.WatchMovies)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<WatchMovie>()
               .HasOne(x => x.User)
               .WithMany(x => x.WatchMovies)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            //table FavoriteMovie
            builder.Entity<FavoriteMovie>()
                .HasKey(x => new { x.UserId, x.MovieId });
            builder.Entity<FavoriteMovie>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.FavoriteMovies)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FavoriteMovie>()
                .HasOne(x => x.User)
                .WithMany(x => x.FavoriteMovies)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);



            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });
            });


            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityUserLogin<string>>();

            builder.Entity<SimilarMovie>()
        .HasKey(sm => new { sm.MovieId, sm.SimilarMovieId });

            builder.Entity<SimilarMovie>()
                .HasOne(sm => sm.Movie)
                .WithMany()
                .HasForeignKey(sm => sm.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SimilarMovie>()
                .HasOne(sm => sm.SimilarMovieEntity)
                .WithMany()
                .HasForeignKey(sm => sm.SimilarMovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Video>()
                .HasOne(x => x.movie)
                .WithMany(m => m.Videos)
                .HasForeignKey(x => x.IdMovie)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<BelongsToCollection> BelongsToCollections { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<FavoriteMovie> FavoriteMovies { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public DbSet<ProductionCountry> ProductionCountries { get; set; }
        public DbSet<RatingList> RatingLists { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SpokenLanguage> SpokenLanguages { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<WatchMovie> WatchMovies { get; set; }
        public DbSet<Movie> SimilarMovies { get; set; }
    }
}
