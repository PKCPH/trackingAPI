namespace trackingAPI.Models;

public class GameMatch
{
    public GameMatch()
    {
        this.ParticipatingTeams = new HashSet<Team>();
    }

    public int Id { get; set; }
    public ICollection<Team> ParticipatingTeams { get; set; }
    public int TeamAScore { get; set; }
    public int TeamBScore { get; set; } //string Score for both teams instead ?
    public MatchState MatchState { get; set; } //0 MatchNotStarted, 1 MatchInPlay, 2 MatchFinished
    public DateTime DateOfMatch { get; set; }
}

public enum MatchState
{
    NotStarted, Playing, Finished
}
