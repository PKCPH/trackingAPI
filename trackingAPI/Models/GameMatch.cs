using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using trackingAPI.Data;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;

namespace trackingAPI.Models;

public class Gamematch
{
    public Gamematch(DatabaseContext databaseContext)
    {
        this.ParticipatingTeams = new HashSet<MatchTeam>();
        
    }
    public Gamematch()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ICollection<MatchTeam> ParticipatingTeams { get; set; }
    public MatchState MatchState { get; set; } //0 MatchNotStarted, 1 MatchInPlay, 2 MatchFinished
    public DateTime DateOfMatch { get; set; }

    /// <summary>
    /// League relevant properties
    /// </summary>

    public League? League { get; set; }
    [DefaultValue(true)]
    public bool IsDrawAllowed { get; set; }
    public int TeamASeed { get; set; }
    public int TeamBSeed { get; set; }
    public bool IsLeagueGame { get; set; }
    //public Team Winner { get; set; }
    //public int WinnerSeed => this.ParticipatingTeams.First(x => x.Team == this.Winner).Seed;
}

public enum MatchState
{
    NotStarted, Playing, Finished
}
