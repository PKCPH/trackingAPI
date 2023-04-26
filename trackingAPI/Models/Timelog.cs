using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models
{
    public class Timelog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public Gamematch Gamematch { get; set; }
        
        public MatchState MatchState { get; set; }
    }

}
