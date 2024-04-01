using System.ComponentModel.DataAnnotations;

namespace MovieRecommendationApi.Dtos
{
    public class CreateMovieDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Genre { get; set; } = "none";
    }
}