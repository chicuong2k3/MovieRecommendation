using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
	public class Video
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }
		public string Name {  get; set; }
		public string Site {  get; set; }
		public string Type { get; set; }
		public bool Official { get; set; }
		public DateOnly PublishedAt { get; set; }
	}
}
