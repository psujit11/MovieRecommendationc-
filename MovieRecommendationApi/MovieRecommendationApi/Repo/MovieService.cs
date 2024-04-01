using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Data;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Models;

namespace MovieRecommendationApi.Repo
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadMovieDto>> GetAllMoviesAsync()
        {
            var movies = await _context.Movies.ToListAsync();
            return _mapper.Map<IEnumerable<ReadMovieDto>>(movies);
        }

        public async Task<ReadMovieDto> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                throw new Exception("Movie not found");
            return _mapper.Map<ReadMovieDto>(movie);
        }

        public async Task<IEnumerable<ReadMovieDto>> GetMoviesByGenreAsync(string genre)
        {
            var movies = await _context.Movies.Where(m => m.Genre == genre).ToListAsync();
            return movies == null ? throw new Exception("Movie not found") : _mapper.Map<IEnumerable<ReadMovieDto>>(movies);
        }

        public async Task<Movie> UpdateMovieAsync(UpdateMovieDto movieDto)
        {
            var movie = await _context.Movies.FindAsync(movieDto.ID);
            if (movie == null)
                throw new Exception("Movie not found");

            _mapper.Map(movieDto, movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                throw new Exception("Movie not found");

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            try
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return movie;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                throw new Exception("Failed to add the movie.", ex);
            }
        }
    }
}

