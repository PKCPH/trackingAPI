using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models;

public class Team
{

    public Team()
    {
        this.Matches = new HashSet<MatchTeam>();
        this.Players = new HashSet<PlayerTeam>();
        this.Leagues = new HashSet<LeagueTeam>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    public decimal? Rating { get; set; }

    [DefaultValue("true")]
    public bool? IsAvailable { get; set; }
    public ICollection<MatchTeam>? Matches { get; set; }
    public ICollection<PlayerTeam>? Players { get; set; }
    public ICollection<LeagueTeam>? Leagues { get; set; }
}
