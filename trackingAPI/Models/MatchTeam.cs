using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models
{
    public class MatchTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("GameMatch")]
        public Guid MatchId { get; set; }
        public GameMatch Match { get; set; }

        [ForeignKey("Team")]
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }
}
