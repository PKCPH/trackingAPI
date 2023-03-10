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
    public MatchState MatchState { get; set; }
    public DateTime DateOfMatch { get; set; }

    [DefaultValue(true)]
    public bool IsDrawAllowed { get; set; }

    /// <summary>
    /// League relevant properties
    /// </summary>

    public League? League { get; set; }
    public bool IsLeagueGame { get; set; }

    
}

public enum MatchState
{
    NotStarted, Playing, Finished
}
