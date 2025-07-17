using Microsoft.EntityFrameworkCore;
using CovidTracker.Domain.Models;

namespace CovidTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<StateStat> StateStats { get; set; }
    public DbSet<CovidAlert> CovidAlerts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<StateStat>()
            .HasKey(ss => new { ss.State, ss.Timestamp });
        modelBuilder.Entity<CovidAlert>()
            .HasKey(ca => new { ca.State, ca.Time });
    }
}
