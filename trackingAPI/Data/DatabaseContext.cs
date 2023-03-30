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

    public DbSet<Team> Teams { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Gamematch> Matches { get; set; }

    public DbSet<LeagueTeam> LeagueTeams { get; set; }
    public DbSet<MatchTeam> MatchTeams { get; set; }
    public DbSet<PlayerTeam> PlayerTeams { get; set; }
    public DbSet<Bet> Bets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Login>().HasData(new Login
        {
            UserName = "admin",
            Password = "123456",
            Role = "Admin"
        });

        modelBuilder.Entity<Login>()
             .Property(t => t.Role)
            .HasDefaultValue("User");

        modelBuilder.Entity<Login>().HasIndex(u => u.UserName).IsUnique();

        //Default values
        modelBuilder.Entity<Bet>()
            .Property(b => b.BetResult)
            .HasDefaultValue(BetResult.Undetermined);
        modelBuilder.Entity<Bet>()
            .Property(b => b.BetState)
            .HasDefaultValue(BetState.InProgress);
        modelBuilder.Entity<Bet>()
            .Property(b => b.Odds)
            .HasColumnType("decimal(18, 4)");
        modelBuilder.Entity<Team>()
            .Property(t => t.IsAvailable)
            .HasDefaultValue(true);
        modelBuilder.Entity<MatchTeam>()
            .Property(gm => gm.Result)
            .HasDefaultValue(Result.Undetermined);
        modelBuilder.Entity<Team>()
            .Property(t => t.Rating)
            .HasColumnType("decimal(18, 4)");
        modelBuilder.Entity<Gamematch>()
            .Property(g => g.Odds)
            .HasColumnType("decimal(18, 4)");
    }
}
