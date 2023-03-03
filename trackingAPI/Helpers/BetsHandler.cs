using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public class BetsHandler
    {
        private readonly IServiceProvider _services;
        public BetsHandler(IServiceProvider services)
        {
            _services = services;
        }

        // Service method to update user balances based on match results
        public async Task UpdateBalancesOnMatchFinish(GameMatch match)
        {
            using (var scope = _services.CreateScope())
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<DatabaseContext>();
                // Find the match
                if (match == null)
                {
                    return;
                }

                // Calculate payouts for winning bets
                var winningTeam = DetermineWinningTeam(match);

                   var bets = _context.Bets.Where(b => b.GameMatchId == match.Id).ToList(); 
                   var winningBets = bets.Where(b => b.Team == winningTeam).ToList();
                    
                    foreach (var bet in winningBets)
                    {
                        var user = await _context.Logins.FindAsync(bet.LoginId);
                        if (user != null)
                        {
                            user.Balance += bet.Amount * 2;
                            /* user.Balance += bet.Amount * match.GetOddsForTeam(bet.Team);*/
                        }
                    }

                // Calculate refunds for draw bets
                var drawBets = bets.Where(b => b.Team == null);
                foreach (var bet in drawBets)
                {
                    var user = await _context.Logins.FindAsync(bet.LoginId);
                    if (user != null)
                    {
                        user.Balance += bet.Amount;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public string DetermineWinningTeam(GameMatch match)
        {
            var winningTeamName = "";

            foreach (var item in match.ParticipatingTeams)
            {
                if (item.Result == Result.Winner)
                {
                     winningTeamName = item.Team.Name;
                }
                else if (item.Result == Result.Draw)
                {
                    winningTeamName = null;
                }

            }

            return winningTeamName;
        }
    }
}
