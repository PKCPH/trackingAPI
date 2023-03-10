using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using trackingAPI.Data;
using System.ComponentModel;

namespace trackingAPI.Models;

public class League
{
    public League(DatabaseContext databaseContext)
    {
        this.Teams= new HashSet<LeagueTeam>();
        this.Gamematches = new HashSet<Gamematch>();
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }

    [DefaultValue(LeagueState.NotStarted)]
    public LeagueState LeagueState { get; set; }
    public DateTime StartDate { get; set; }
    public ICollection<LeagueTeam> Teams { get; set; }
    //public ICollection<LeagueGamematchRound> MatchLeagueRounds { get; set; }

    public ICollection<Gamematch> Gamematches { get; set; }


}

public enum LeagueState
{
    NotStarted, InProgress, Finished
}
