using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using trackingAPI.Data;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;

namespace trackingAPI.Models;

public class GameMatch
{
    public GameMatch(DatabaseContext databaseContext)
    {
        this.ParticipatingTeams = new HashSet<MatchTeam>();
    }
    public GameMatch()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ICollection<MatchTeam> ParticipatingTeams { get; set; }
    public MatchState MatchState { get; set; } //0 MatchNotStarted, 1 MatchInPlay, 2 MatchFinished
    public DateTime DateOfMatch { get; set; }
    public League? League { get; set; }
    [DefaultValue(true)]
    public bool IsDrawAllowed { get; set; }
}

public enum MatchState
{
    NotStarted, Playing, Finished
}
