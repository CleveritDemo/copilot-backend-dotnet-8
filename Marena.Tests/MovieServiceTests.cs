using Marena.API.Models;
using Marena.API.Persistence;
using Marena.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class MovieServiceTests : IDisposable
{
    private readonly MarenaDBContext _context;
    private readonly IMovieService _movieService;

    public MovieServiceTests()
    {
        var options = new DbContextOptionsBuilder<MarenaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MarenaDBContext(options);
        _movieService = new MovieService(_context);
    }

    [Fact]
    public async Task GetMoviesAsync_ReturnsAllMovies()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Name = "Movie 1", Score = 8.5, Genres = "Action", Year = 2020 },
            new Movie { Id = 2, Name = "Movie 2", Score = 7.0, Genres = "Drama", Year = 2019 }
        };

        await _context.Movies.AddRangeAsync(movies);
        await _context.SaveChangesAsync();

        // Act
        var result = await _movieService.GetMoviesAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetMovieByIdAsync_ReturnsMovie()
    {
        // Arrange
        var movie = new Movie { Id = 1, Name = "Movie 1", Score = 8.5, Genres = "Action", Year = 2020 };
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        // Act
        var result = await _movieService.GetMovieByIdAsync(1);

        // Assert
        Assert.Equal(movie, result);
    }

    [Fact]
    public async Task AddMovieAsync_AddsMovie()
    {
        // Arrange
        var movie = new Movie { Id = 1, Name = "Movie 1", Score = 8.5, Genres = "Action", Year = 2020 };

        // Act
        var result = await _movieService.AddMovieAsync(movie);

        // Assert
        var addedMovie = await _context.Movies.FindAsync(movie.Id);
        Assert.Equal(movie, addedMovie);
    }

    [Fact]
    public async Task UpdateMovieAsync_UpdatesMovie()
    {
        // Arrange
        var movie = new Movie { Id = 1, Name = "Movie 1", Score = 8.5, Genres = "Action", Year = 2020 };
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        movie.Name = "Updated Movie 1";

        // Act
        var result = await _movieService.UpdateMovieAsync(1, movie);

        // Assert
        var updatedMovie = await _context.Movies.FindAsync(movie.Id);
        Assert.Equal("Updated Movie 1", updatedMovie.Name);
    }

    [Fact]
    public async Task DeleteMovieAsync_DeletesMovie()
    {
        // Arrange
        var movie = new Movie { Id = 1, Name = "Movie 1", Score = 8.5, Genres = "Action", Year = 2020 };
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        // Act
        var result = await _movieService.DeleteMovieAsync(1);

        // Assert
        var deletedMovie = await _context.Movies.FindAsync(1);
        Assert.Null(deletedMovie);
        Assert.True(result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
