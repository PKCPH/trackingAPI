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
    public DbSet<Login> Logins { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerTeam> PlayerTeams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Login>().HasData(new Login
        //{
        //    UserName = "admin",
        //    Password = "123456",
        //    Role = "Admin"
        //});

        modelBuilder.Entity<Login>()
             .Property(t => t.Role)
            .HasDefaultValue("User");

        modelBuilder.Entity<Login>().HasIndex(u => u.UserName).IsUnique();

        //Default value for IsAvailable = true
        modelBuilder.Entity<Team>()
            .Property(t => t.IsAvailable)
            .HasDefaultValue(true);
        modelBuilder.Entity<GameMatch>()
            .Property(gm => gm.IsDrawAllowed)
            .HasDefaultValue(true);
        modelBuilder.Entity<MatchTeam>()
            .Property(gm => gm.Result)
            .HasDefaultValue(Result.Undetermined);

    }
}
