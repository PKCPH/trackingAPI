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
        this.ParticipatingTeams = new HashSet<GamematchTeam>();
        
    }
    public Gamematch()
    {
        Bets = new List<Bet>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ICollection<GamematchTeam>? ParticipatingTeams { get; set; }
    public MatchState MatchState { get; set; } //0 MatchNotStarted, 1 MatchInPlay, 2 MatchFinished
    public DateTime DateOfMatch { get; set; }

    [DefaultValue(true)]
    public bool IsDrawAllowed { get; set; }
    public Guid? LeagueId { get; set; }
    public League league { get; set; }
    public ICollection<Bet> Bets { get; set; }
    public int? Round { get; set; }
}

public enum MatchState
{
    NotStarted, FirstHalf, HalfTimePause, SecondHalf, OverTime, PenaltyShootOut, Finished
}
