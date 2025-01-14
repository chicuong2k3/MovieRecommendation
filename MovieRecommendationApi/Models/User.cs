using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = default!;
        public string? Email { get; set; } = default!;
        public string? Name { get; set; }
        public string? AvatarPath { get; set; }
    }


}
