using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using trackingAPI.Data;
using System.Security.Cryptography.X509Certificates;

namespace trackingAPI.Models;

public class GameMatch
{
    public GameMatch(DatabaseContext databaseContext)
    {
        this.ParticipatingTeams = new HashSet<MatchTeam>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ICollection<MatchTeam> ParticipatingTeams { get; set; }
    public int TeamAScore { get; set; }
    public int TeamBScore { get; set; } //string Score for both teams instead ?
    public MatchState MatchState { get; set; } //0 MatchNotStarted, 1 MatchInPlay, 2 MatchFinished.
    public DateTime DateOfMatch { get; set; }
}

public enum MatchState
{
    NotStarted, Playing, Finished
}
