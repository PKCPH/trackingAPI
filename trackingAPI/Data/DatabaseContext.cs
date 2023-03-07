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
    //public DbSet<LeagueGamematch> LeagueMatches { get; set; }

    //public DbSet<MatchupModel> MatchupModels { get; set; }
    //public DbSet<MatchupEntryModel> MatchupEntryModels { get; set; }
    
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
        modelBuilder.Entity<Gamematch>()
            .Property(gm => gm.IsDrawAllowed)
            .HasDefaultValue(true);
        modelBuilder.Entity<MatchTeam>()
            .Property(gm => gm.Result)
            .HasDefaultValue(Result.Undetermined);

        modelBuilder.Entity<LeagueTeam>()
                .Property(lt => lt.InTournament)
                .HasDefaultValue(true);

        //modelBuilder.Entity<League>()
        //.HasMany(l => l.Rounds)
        //.WithOne(r => r.League)
        //.HasForeignKey<MatchLeagueRounds>(r => r.League.Id);

        //modelBuilder.Entity<MatchupModel>()
        //.HasNoKey();

        //modelBuilder.Entity<MatchupModel>()
        //.HasMany(m => m.Entries)
        //.WithOne(e => e.ParentMatchup)
        //.HasForeignKey(e => e.ParentMatchupId);

        // Configure the relationship between MatchupModel and MatchupEntryModel
        //modelBuilder.Entity<MatchupEntryModel>()
        //    .HasOne(m => m.ParentMatchup)
        //    .WithMany(m => m.Entries)
        //    .HasForeignKey(m => m.ParentMatchupId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //modelBuilder.Entity<MatchupModel>().HasNoKey();
        //modelBuilder.Entity<MatchupEntryModel>().HasNoKey();
    }
}
