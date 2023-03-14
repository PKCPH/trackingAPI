using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models
{
    public class Bet
    {

        public Guid Id { get; set; }

        [ForeignKey("GameMatch")]

        public Guid MatchId { get; set; }

        public Gamematch? Match { get; set; } //Navigation object

        [ForeignKey("Login")]
        public Guid LoginId { get; set; }
        public string Team { get; set; }

        // Can be empty string or "draw" for a draw
        // Instead of doing a team/player/draw u vote on as a string pass ID's instead
        public int Amount { get; set; }

        public BetResult BetResult { get; set; }

        public BetState BetState { get; set; }

        public Bet()
        {
            Match = new Gamematch();
        }
    }

    public enum BetState
    {
        InProgress, Finished
    }

    public enum BetResult
    {
        Undetermined, Loss, Win
    }
}
