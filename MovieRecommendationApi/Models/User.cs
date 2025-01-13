using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Role { set; get; }

        public IList<RatingList>? RatingLists { set; get; }
        public IList<Review>? Reviews { set; get; }
        public IList<WatchMovie>? WatchMovies { set; get; }
        public IList<FavoriteMovie>? FavoriteMovies { set; get; }
    }
}
