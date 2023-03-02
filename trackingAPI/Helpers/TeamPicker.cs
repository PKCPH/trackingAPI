using System.Text.RegularExpressions;
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

        //var AvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList();
        //var TwoRandomAvailableTeams = AvailableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();

        var AvailableTeams = _context.LeagueTeams.Where(x => x.LeagueId == league.Id).ToList();
        var TwoRandomAvailableTeams = AvailableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();

        //var listOfRounds = Generate(8);
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
}
