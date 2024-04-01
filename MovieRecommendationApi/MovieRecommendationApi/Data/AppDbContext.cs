using Microsoft.EntityFrameworkCore;
using MovieRecommendationApi.Models;

namespace MovieRecommendationApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }
    }
}

