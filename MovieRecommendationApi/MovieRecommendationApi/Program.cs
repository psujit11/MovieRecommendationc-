using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; 
using MovieRecommendationApi.Data;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Repo;
using AutoMapper;
using MovieRecommendationApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/v1/Movies", async (IMovieService movieservice, IMapper mapper) =>
    {
        var movies = await movieservice.GetAllMoviesAsync();
        return Results.Ok(mapper.Map<IEnumerable<ReadMovieDto>>(movies));

    });

app.MapPost("api/v1/Movies", async (IMovieService movieService, CreateMovieDto createMovieDto, IMapper mapper) =>
{
    try
    {
        var movie = mapper.Map<Movie>(createMovieDto);
        var newMovie = await movieService.AddMovieAsync(movie);
        return Results.Ok(mapper.Map<ReadMovieDto>(newMovie));
    }
    catch (Exception ex)
    {
        
        return Results.Problem(ex.Message);
    }
});
app.MapGet("api/v1/Movies/genre/{genre}", async (IMovieService movieService, string genre, IMapper mapper) =>
{
    try
    {
        var movies = await movieService.GetMoviesByGenreAsync(genre);
        return Results.Ok(mapper.Map<IEnumerable<ReadMovieDto>>(movies));
    }
    catch (Exception ex)
    {
        
        return Results.Problem(ex.Message);
    }
});

app.MapGet("api/v1/Movies/{id}", async (IMovieService movieService, int id, IMapper mapper) =>
{
    try
    {
        var movie = await movieService.GetMovieByIdAsync(id);
        return Results.Ok(mapper.Map<ReadMovieDto>(movie));
    }
    catch (Exception ex)
    {
        // Log the exception if necessary
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("api/v1/Movies/{id}", async (IMovieService movieService, int id) =>
{
    try
    {
        await movieService.DeleteMovieAsync(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        // Log the exception if necessary
        return Results.Problem(ex.Message);
    }
});

app.MapPut("api/v1/Movies/{id}", async (IMovieService movieService, int id, UpdateMovieDto movieDto, IMapper mapper) =>
{
    try
    {
        movieDto.ID = id;
        var updatedMovie = await movieService.UpdateMovieAsync(movieDto);
        return Results.Ok(mapper.Map<ReadMovieDto>(updatedMovie));
    }
    catch (Exception ex)
    {
        // Log the exception if necessary
        return Results.Problem(ex.Message);
    }
});



app.Run();
