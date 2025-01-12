using MovieRecommendationApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieRecommendationApi.Models
{
    public class Credit
    {
        [Key]
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [JsonPropertyName("cast")]
        public List<Person>? Cast { get; set; }
    }
}
