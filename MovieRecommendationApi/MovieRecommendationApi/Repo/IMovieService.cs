using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Models;

namespace MovieRecommendationApi.Repo
{
    public interface IMovieService
    {
        Task<IEnumerable<ReadMovieDto>> GetAllMoviesAsync();
        Task<ReadMovieDto> GetMovieByIdAsync(int id);
        Task<IEnumerable<ReadMovieDto>> GetMoviesByGenreAsync(string genre);
        Task<Movie> UpdateMovieAsync(UpdateMovieDto movieDto);
        Task DeleteMovieAsync(int id);
        Task<Movie> AddMovieAsync(Movie movie);
    }
}
