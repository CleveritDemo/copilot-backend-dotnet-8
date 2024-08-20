using Microsoft.EntityFrameworkCore;

public class MarenaDBContext : DbContext
{
    public MarenaDBContext(DbContextOptions<MarenaDBContext> options) : base(options)
    {
    }

    // Define DbSet properties for your entities here
    public DbSet<Movie> Movies { get; set; }
}
