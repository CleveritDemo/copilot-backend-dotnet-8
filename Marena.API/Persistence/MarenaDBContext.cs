using Marena.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Marena.API.Persistence;

public class MarenaDBContext : DbContext
{
    public MarenaDBContext(DbContextOptions<MarenaDBContext> options) : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }
}
