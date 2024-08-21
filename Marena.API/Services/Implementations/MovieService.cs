using Marena.API.Models;
using Marena.API.Persistence;
using Marena.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class MovieService : IMovieService
{
    private readonly MarenaDBContext _context;

    public MovieService(MarenaDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        return await _context.Movies.ToListAsync();
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        return await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Movie> AddMovieAsync(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<Movie> UpdateMovieAsync(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return null;
        }

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieExistsAsync(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return movie;
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return false;
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> MovieExistsAsync(int id)
    {
        return await _context.Movies.AnyAsync(e => e.Id == id);
    }
}
