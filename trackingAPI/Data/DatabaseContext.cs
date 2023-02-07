using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using trackingAPI.Models;

namespace trackingAPI.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<MatchTeam> MatchTeams { get; set; }
    public DbSet<GameMatch> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Default value for IsAvailable = true
        modelBuilder.Entity<Team>()
            .Property(t => t.IsAvailable)
            .HasDefaultValue(true);
    }
}
