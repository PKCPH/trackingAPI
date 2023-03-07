using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models
{
    public class Bet
    {
        public Guid Id { get; set; }

        [ForeignKey("GameMatch")]

        /*public Guid GameMatchId { get; set; }*/
        public GameMatch Match { get; set; }

        [ForeignKey("Login")]
        public Guid LoginId { get; set; }
        public string Team { get; set; }

        // Can be empty string or "draw" for a draw
        // Instead of doing a team/player/draw u vote on as a string pass ID's instead
        public int Amount { get; set; }

        public Result Result { get; set; }

        public BetState BetState { get; set; }
    }

    public enum BetState
    {
        InProgress, Finished
    }

    public enum BetResult
    {
        Loss, Win
    }
}
