using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using trackingAPI.Data;

namespace trackingAPI.Models;

public class MatchTeam
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Gamematch Match { get; set; }
    public Team? Team { get; set; }
    //public League? League { get; set; }
    public int TeamScore { get; set; }
    [DefaultValue(Result.Undetermined)]
    public Result Result { get; set; }
    public int? Seed { get; set; }
    public int? Round { get; set; }
}

public enum Result
{
    Undetermined, Winner, Loser, Draw
}
