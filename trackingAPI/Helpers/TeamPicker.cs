using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class TeamPicker
{
    //Read list of teams and choose random
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

            gameMatch.DateOfMatch = DateTime.Now.AddHours(1);
            Console.WriteLine("teampicker ended");
        }
        else
        {
            throw new Exception("Could not find enough available teams");
        }

        
        return gameMatch;
    }
}
