using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class TeamPicker
{
    //Read list of teams and choose random
    public GameMatch CreateMatch(DatabaseContext _context)
    {
        GameMatch gameMatch = new();
        Random rnd = new Random();

        var AvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList();
        var TwoRandomAvailableTeams = AvailableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();

        if (TwoRandomAvailableTeams.Count().Equals(2))
        {
            var teamA = TwoRandomAvailableTeams.First();
            var teamB = TwoRandomAvailableTeams.Last();
            teamA.IsAvailable = false;
            teamB.IsAvailable = false;
            gameMatch.ParticipatingTeams.Add(teamA);
            gameMatch.ParticipatingTeams.Add(teamB);

            gameMatch.DateOfMatch = DateTime.Now.AddHours(1);
        }
        else
        {
            throw new Exception("Could not find enough available teams");
        }

        
        return gameMatch;
    }
}
