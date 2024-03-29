﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.BackgroundHelpers;
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
        public async Task UpdateBalancesOnMatchFinish(Gamematch match)
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

                var bets = _context.Bets.Where(b => b.Match.Id == match.Id).ToList();
                var winningBets = bets.Where(b => b.Team == winningTeam).ToList();
                var losingBets = bets.Where(b => b.Team != winningTeam).ToList();

                foreach (var bet in winningBets)
                {
                    var user = await _context.Logins.FindAsync(bet.LoginId);
                    if (user != null)
                    {
                        double userEarning = bet.Amount * 2;

                        //how many percent the house takes from the users profit, as a decimal number
                        double houseTake = 0;

                        userEarning -= (userEarning - bet.Amount) * houseTake;
                        user.Balance += Convert.ToInt32(userEarning);
                        bet.BetResult = BetResult.Win;
                        bet.BetState = BetState.Finished;


                    }
                }

                foreach (var bet in losingBets)
                {
                    bet.BetResult = BetResult.Loss;
                    bet.BetState = BetState.Finished;
                }

                await _context.SaveChangesAsync();
            }
        }

        public string DetermineWinningTeam(Gamematch match)
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
                    winningTeamName = "draw";
                }

            }

            return winningTeamName;
        }
    }
}
