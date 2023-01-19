﻿namespace trackingAPI.Models;

public class Match
{

    public int Id { get; set; }
    public string TeamA { get; set; }
    public string TeamB { get; set; }
    public int TeamAScore { get; set; }
    public int TeamBScore { get; set; } //string Score for both teams instead ?
    public MatchState MatchState { get; set; } //0 MatchNotStarted, 1 MatchInPlay, 2 MatchFinished
    public DateTime DateOfMatch { get; set; }
    public List<Team> Teams { get; set; }
}

public enum MatchState
{
    NotStarted, Playing, Finished
}
