using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models;

public class LeagueGamematchRound
{
    [Key]
    public int Id { get; set; }
    public League League { get; set; }
    public ICollection<Gamematch> Matches { get; set; }
}
