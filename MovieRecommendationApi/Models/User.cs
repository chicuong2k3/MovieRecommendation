using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Role { set; get; }

        public ICollection<RatingList> RatingLists { set; get; }
        public ICollection<Review> Reviews { set; get; }
        public ICollection<WatchMovie> WatchMovies { set; get; }
        public ICollection<FavoriteMovie> FavoriteMovies { set; get; }
    }
}
