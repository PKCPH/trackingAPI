using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models
{
    public class LeagueMatches
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        // where number is player's seed number        
        public int PlayerA { get; set; }
        public int PlayerB { get; set; }
        public Round Rounds { get; set; }
    }
}
