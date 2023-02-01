using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public class TeamPicker
    {
        private readonly DatabaseContext _context;

        //Read list of teams and choose random
        public GameMatch CreateMatch(DatabaseContext _context)
        {
            GameMatch gameMatch = new();
            Random rnd = new Random();

            //var teamCount = _context.Teams.Count();

            var rndTeamsAvailable = _context.Teams.Where(t => (bool)t.IsAvailable).ToList();

            var rndTeams = rndTeamsAvailable.OrderBy(x => rnd.Next()).Take(2).ToList();

            //int idNumberA = 20;
            //int idNumberB = 20;


            var teamA = rndTeams.First();
            var teamB = rndTeams.Last();
            gameMatch.ParticipatingTeams.Add(teamA);
            gameMatch.ParticipatingTeams.Add(teamB);


            return gameMatch;
        }

    }
}
