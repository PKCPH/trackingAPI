using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public class TeamPicker
    {
        private readonly DatabaseContext _context;

        //Read list of teams and choose random
        public GameMatch CreateMatch()
        {
            GameMatch gameMatch = new();

            int idNumberA = 20;
            int idNumberB = 20;

            var teamA = _context.Teams.Find(idNumberA);
            var teamB = _context.Teams.Find(idNumberB);
            gameMatch.ParticipatingTeams.Add(teamA);
            gameMatch.ParticipatingTeams.Add(teamB);


            return gameMatch;
        }
    }
}
