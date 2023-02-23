using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models;

public class League
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string Sport { get; set; }
    //public LeagueState LeagueState { get; set; }
    //public DateTime StartDate { get; set; }
    ////maybe timeSpan instead of enddate
    //public DateTime EndDate { get; set; }
    ////maybe add LeagueFormat?.
    //public ICollection<Team> LeagueTeams { get; set; }
    //public int NumberOfTeams { get; set; }
}

public enum LeagueState
{
    NotStarted, InProgress, Finished
}
