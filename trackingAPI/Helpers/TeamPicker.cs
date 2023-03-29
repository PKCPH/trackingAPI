using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class TeamPicker
{

    //Read list of teams and choose two random team to be put in ParticipatingTeams.
    public Gamematch CreateMatch(DatabaseContext _context)
    {


        Gamematch gameMatch = new(_context)
        {
            IsDrawAllowed = true,
        };


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

            TimeLog timeLog = new TimeLog
            {
                StartDateTime = gameMatch.DateOfMatch,
                TimeStamp = TimeSpan.Zero,
                Event = "Not Started",
                Gamematch = gameMatch
            };

            gameMatch.TimeLog.Add(timeLog);
            Console.WriteLine($"MATCH CREATED: {matchTeamA.Team.Name} VS. {matchTeamB.Team.Name}");
        }
        else
        {
            throw new Exception("Could not find enough available teams");
        }
        return gameMatch;
    }
}
