using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string Genre { get; set; }    
    }
}
