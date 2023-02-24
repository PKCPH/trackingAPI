using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models;

public class LeagueTeam
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [ForeignKey("League")]
    public Guid LeagueId { get; set; }
    [ForeignKey("Team")]
    public Guid TeamId { get; set; }
}


