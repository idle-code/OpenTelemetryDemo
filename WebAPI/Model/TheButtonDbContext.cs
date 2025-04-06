using Microsoft.EntityFrameworkCore;

namespace WebAPI.Model;

public class TheButtonDbContext : DbContext
{
    public DbSet<NamedCounter> NamedCounters { get; set; }
    public DbSet<BonusToken> BonusTokens { get; set; }

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

        modelBuilder.Entity<BonusToken>(token =>
        {
            token.HasKey(t => new { t.CounterId, t.Token });
            token.Property(t => t.Token)
                .HasMaxLength(128);

            token.HasOne<NamedCounter>()
                .WithMany()
                .HasForeignKey(t => t.CounterId)
                .HasPrincipalKey(c => c.Id);

            token.Property(t => t.ValidUntil)
                .IsRequired();

            token.ToTable(nameof(BonusToken), "TheButton");
        });
    }
}
