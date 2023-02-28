﻿using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class TeamPicker
{
    //Read list of teams and choose two random team to be put in ParticipatingTeams.
    public GameMatch CreateMatch(DatabaseContext _context)
    {
        GameMatch gameMatch = new(_context);
        Random rnd = new Random();

        var AvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList();
        var TwoRandomAvailableTeams = AvailableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();

        if (TwoRandomAvailableTeams.Count().Equals(2))
        {
            var teamA = TwoRandomAvailableTeams.First();
            var teamB = TwoRandomAvailableTeams.Last();
            teamA.IsAvailable = false;
            teamB.IsAvailable = false;
            
            MatchTeam matchTeamA = new MatchTeam { Team = teamA };
            MatchTeam matchTeamB = new MatchTeam { Team = teamB };

            gameMatch.ParticipatingTeams.Add(matchTeamA);
            gameMatch.ParticipatingTeams.Add(matchTeamB);

            gameMatch.DateOfMatch = DateTimePicker.CreateRandomMatchTime();
            Console.WriteLine($"MATCH CREATED: {matchTeamA.Team.Name} VS. {matchTeamB.Team.Name}");
        }
        else
        {
            throw new Exception("Could not find enough available teams");
        }
        return gameMatch;
    }
    //For Leagues
    public GameMatch CreateMatch(DatabaseContext _context, League league)
    {
        GameMatch gameMatch = new(_context);
        Random rnd = new Random();
        var listOfRounds = Generate(16);

        //var AvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList();
        //var TwoRandomAvailableTeams = AvailableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();

        var AvailableTeams = _context.LeagueTeams.Where(x => x.LeagueId == league.Id).ToList();
        var TwoRandomAvailableTeams = AvailableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();

        if (TwoRandomAvailableTeams.Count().Equals(2))
        {
            var teamA = TwoRandomAvailableTeams.First().TeamId;
            var teamB = TwoRandomAvailableTeams.Last().TeamId;

            MatchTeam matchTeamA = new MatchTeam { TeamId = teamA };
            MatchTeam matchTeamB = new MatchTeam { TeamId = teamB };

            gameMatch.ParticipatingTeams.Add(matchTeamA);
            gameMatch.ParticipatingTeams.Add(matchTeamB);

            gameMatch.DateOfMatch = DateTimePicker.CreateRandomMatchTime();
            Console.WriteLine($"MATCH CREATED: {matchTeamA.Team.Name} VS. {matchTeamB.Team.Name}");
        }
        else
        {
            throw new Exception("Could not find enough available teams");
        }
        return gameMatch;
    }
    
    //generating bracketrounds for single elimination tournaments
    static Round[] Generate(int teamNumber, Team[] teams)
    {
        // only works for power of 2 number of players   
        var roundsNumber = (int)Math.Log(teamNumber, 2);
        var rounds = new Round[roundsNumber];
        for (int i = 0; i < roundsNumber; i++)
        {
            var round = new Round();
            var prevRound = i > 0 ? rounds[i - 1] : null;
            if (prevRound == null)
            {
                // if first round - result is known
                round.Matches = new[] {
                    new GameMatch() {
                        TeamASeed = 1,
                        TeamBSeed = 2
                    }
                };
            }
            else
            {
                round.Matches = new GameMatch[prevRound.Matches.Length * 2];
                // find median. For round 2 there are 4 players and median is 2.5 (between 2 and 3)
                // for round 3 there are 8 players and median is 4.5 (between 4 and 5)
                var median = (round.Matches.Length * 2 + 1) / 2f;
                var next = 0;
                foreach (var match in prevRound.Matches)
                {
                    // you can play here by switching PlayerA and PlayerB or reordering stuff
                    round.Matches[next] = new GameMatch()
                    {
                        TeamASeed = match.TeamASeed,
                        TeamBSeed = (int)(median + Math.Abs(match.TeamASeed - median))
                    };
                    next++;
                    round.Matches[next] = new GameMatch()
                    {
                        TeamASeed = match.TeamASeed,
                        TeamBSeed = (int)(median + Math.Abs(match.TeamASeed - median))
                    };
                    next++;
                }
            }
            rounds[i] = round;
        }
        return rounds.Reverse().ToArray();
    }
}
