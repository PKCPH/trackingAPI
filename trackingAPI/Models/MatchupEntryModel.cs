using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace trackingAPI.Models
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// The unique identifier for the matchup entry
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        /// <summary>
        /// Represents one team in the matchup
        /// </summary>
        public Team TeamCompeting { get; set; }
        /// <summary>
        /// Represents the score for this particular team
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Represent the mathup that this team came
        /// from as the winner
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialscore"></param>
        //public MatchupEntryModel(double initialscore)
        //{

        //}
    }
}
