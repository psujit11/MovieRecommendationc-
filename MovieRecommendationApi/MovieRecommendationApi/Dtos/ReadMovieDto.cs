namespace MovieRecommendationApi.Dtos
{
    public class ReadMovieDto
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
    }
}
