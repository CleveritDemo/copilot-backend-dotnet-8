using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marena.API.Services.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetMoviesAsync();
    Task<Movie> GetMovieByIdAsync(int id);
    Task<Movie> AddMovieAsync(Movie movie);
    Task<Movie> UpdateMovieAsync(int id, Movie movie);
    Task<bool> DeleteMovieAsync(int id);
}
