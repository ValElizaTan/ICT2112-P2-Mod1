using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet properties will be added here after ERD finalization
    // Example: public DbSet<YourEntity> YourEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Entity configurations will be added here after ERD finalization
    }
}
