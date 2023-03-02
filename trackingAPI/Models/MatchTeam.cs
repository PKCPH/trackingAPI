using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using trackingAPI.Data;

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
        public int TeamScore { get; set; }

        [DefaultValue(Result.Undetermined)]
        public Result Result { get; set; }
    }

    public enum Result
    {
        Undetermined, Winner, Loser, Draw
    }
}
