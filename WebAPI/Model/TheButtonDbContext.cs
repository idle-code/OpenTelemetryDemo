using Microsoft.EntityFrameworkCore;

namespace WebAPI.Model;

public class TheButtonDbContext : DbContext
{
    public DbSet<NamedCounter> NamedCounters { get; set; }

    public TheButtonDbContext(DbContextOptions<TheButtonDbContext> dbContextOptions)
        : base(dbContextOptions)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NamedCounter>(counter =>
        {
            counter.HasKey(c => c.Id);
            counter.Property(c => c.Id)
                .HasMaxLength(64);

            counter.Property(c => c.Value)
                .HasDefaultValue(0)
                .IsRequired();

            counter.ToTable(nameof(NamedCounters), "TheButton");
        });
    }
}
