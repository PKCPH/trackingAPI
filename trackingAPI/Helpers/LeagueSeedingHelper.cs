using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueSeedingHelper
{
    public League GetListOfEightTeams(League league, DatabaseContext _context)
    {
        Random rnd = new Random();

        var eightAvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList().Take(8);

        foreach (var team in eightAvailableTeams)
        {
            LeagueTeam leagueTeamA = new LeagueTeam { Team = team };
            league.Teams.Add(leagueTeamA);
        }
        return league;
    }


}
