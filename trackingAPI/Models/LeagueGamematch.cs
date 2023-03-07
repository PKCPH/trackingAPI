using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models;

public class LeagueGamematch
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    // where number is player's seed number        
    
    public League League { get; set; }
    public GameMatch Gamematch { get; set; }
    public int TeamASeed { get; set; }
    public int TeamBSeed { get; set; }
    //public Round Rounds { get; set; }
}
