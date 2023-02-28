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
        this.GameMatches= new HashSet<GameMatch>();
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    //public string Sport { get; set; }
    [DefaultValue(LeagueState.NotStarted)]
    public LeagueState LeagueState { get; set; }
    public DateTime StartDate { get; set; }
    ////maybe timeSpan instead of enddate
    //public DateTime EndDate { get; set; }
    ////maybe add LeagueFormat?.
    public ICollection<LeagueTeam> Teams { get; set; }
    //public ICollection<GameMatch> LeagueMatches { get; set; }
    //public int NumberOfTeams { get; set; }
    public ICollection<GameMatch> GameMatches { get; set; }

}

public enum LeagueState
{
    NotStarted, InProgress, Finished
}
