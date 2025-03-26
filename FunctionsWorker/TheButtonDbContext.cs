using Microsoft.EntityFrameworkCore;

namespace FunctionsWorker;

public class TheButtonDbContext : DbContext
{
    public DbSet<TheCounter> Counters { get; set; }

    public TheButtonDbContext(DbContextOptions<TheButtonDbContext> dbContextOptions)
        : base(dbContextOptions)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TheCounter>(counter =>
        {
            counter.HasKey(c => c.Id);

            counter.Property(c => c.Value)
                .HasDefaultValue(0)
                .IsRequired();
        });
    }
}
