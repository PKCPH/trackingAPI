using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class MatchBackgroundTask
{
    private readonly IServiceProvider _services;
    public MatchBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }
    public async Task CreateNewMatchesOfAvailableTeams()
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            if (_context.Teams.Count(x => (bool)x.IsAvailable) > 1)
            {
                TeamPicker teamPicker = new();
                await _context.Matches.AddAsync(teamPicker.CreateMatch(_context));
                await _context.SaveChangesAsync();
            }
        }
    }

    public Task FindAndPlayMatches()
    {
        DateTime now = DateTime.Now;
        //var matches = GetListOfScheduledGameMatchesByDateTime();
        //while any matches has passed the datetime.now
        while (GetListOfScheduledGameMatchesByDateTime().Any(x => x.DateOfMatch < now))
        {
            var firstGameMatch = GetListOfScheduledGameMatchesByDateTime().OrderBy(x => x.DateOfMatch).First();
            // while now has past the schedule time of the match  
            if (now > firstGameMatch.DateOfMatch)
            {
                using (var scope = _services.CreateScope())
                {
                    var _context =
                        scope.ServiceProvider
                            .GetRequiredService<DatabaseContext>();
                    _context.Entry(firstGameMatch).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                //new thread is created and started per livematch
                Thread thread = new Thread(() => { PlayGameMatch(firstGameMatch); });
                thread.Start();
                Console.WriteLine($"*********THREAD #{thread.ManagedThreadId} for MATCH {firstGameMatch.Id} is started");
                Thread.Sleep(100);
            }
        }
        return Task.CompletedTask;
    }

    public IOrderedEnumerable<Gamematch> GetListOfScheduledGameMatchesByDateTime()
    {
        List<Gamematch> gameMatches = new List<Gamematch>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            foreach (var match in _context.Matches.Where(x => x.MatchState == MatchState.NotStarted).ToList())
            {
                match.ParticipatingTeams = _context.MatchTeams.Where(x => x.Match.Id == match.Id).Where(x => x.Team != null).Include(x => x.Team).ToList();
                gameMatches.Add(match);
            }
        }
        var gameMatchesSortByOrder = gameMatches.OrderBy(x => x.DateOfMatch);
        return gameMatchesSortByOrder;
    }

    public async Task PlayGameMatch(Gamematch gameMatch)
    {
        Random random = new Random();
        List<MatchTeam> matchTeams = new List<MatchTeam>();
        LiveMatchBackgroundTask liveMatchBackgroundTask = new(_services);
        //BetsHandler betsHandler = new(_services);
        CancellationToken stoppingToken;

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            //If match has a BYE team
            if(gameMatch.ParticipatingTeams.Any(x => x.Team.Name == "BYE"))
            {
                var team = gameMatch.ParticipatingTeams.Where(x => x.Team.Name != "BYE").First();
                var teamBye = gameMatch.ParticipatingTeams.Where(x => x.Team.Name == "BYE").First();
                team.Result = Result.Winner;
                teamBye.Result = Result.Loser;
                gameMatch.MatchState = MatchState.Finished;
                _context.Entry(team).State = EntityState.Modified;
                _context.Entry(teamBye).State = EntityState.Modified;
                _context.Entry(teamBye.Team).State = EntityState.Deleted;
                _context.Matches.Update(gameMatch);
                await _context.SaveChangesAsync();

                //redundant! put from executelivematch out here in playgamematch
                var nextRound = gameMatch.ParticipatingTeams.Where(x => x.Id == team.Id).First().Round;
                nextRound--;

                //takes lowest int of Seeds
                var winnerSeed = Math.Min(Convert.ToByte(team.Seed), Convert.ToByte(teamBye.Seed));
                var nextMatchTeam = _context.MatchTeams.Where(x => x.Round == nextRound).Where(x => x.Seed == winnerSeed).First();
                //adding winning team
                nextMatchTeam.Team = gameMatch.ParticipatingTeams.Where(x => x.Result == Result.Winner).First().Team;
                //updating to sql
                _context.Entry(nextMatchTeam.Team).State = EntityState.Modified;
                _context.SaveChanges();
                return;
            }

            await liveMatchBackgroundTask.ExecuteLiveMatch(gameMatch);
            gameMatch.MatchState = MatchState.Finished;
            //await betsHandler.UpdateBalancesOnMatchFinish(gameMatch);

            if (gameMatch.LeagueId == null)
            {
                //updating teams to is available
                foreach (var item in gameMatch.ParticipatingTeams)
                {
                    item.Team.IsAvailable = true;
                    _context.Entry(item.Team).State = EntityState.Modified;
                }
                // detach the gameMatch instance to avoid conflicts with the context
                _context.Entry(gameMatch).State = EntityState.Detached;

            }
            else
            {
                var team = gameMatch.ParticipatingTeams.Where(x => x.Result == Result.Loser).First().Team;

                team.IsAvailable = true;
                _context.Entry(team).State = EntityState.Modified;

            }

            // attach the updated gameMatch instance to the context and save changes
            _context.Matches.Update(gameMatch);
            await _context.SaveChangesAsync();

            _context.SaveChanges();
        }
    }

    //replace instead of in match and bye team matches post finished
    private Task UpdateFinishedMatchInDatabase(Gamematch gamematch)
    {
        //match is done here, move out of this method

        var teamA = gamematch.ParticipatingTeams.First();
        var teamB = gamematch.ParticipatingTeams.Last();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            if (teamA.TeamScore > teamB.TeamScore) { teamA.Result = Result.Winner; teamB.Result = Result.Loser; }
            if (teamA.TeamScore < teamB.TeamScore) { teamA.Result = Result.Loser; teamB.Result = Result.Winner; }
            if (teamA.TeamScore == teamB.TeamScore && gamematch.IsDrawAllowed) { teamA.Result = Result.Draw; teamB.Result = Result.Draw; }

            _context.Entry(teamA).State = EntityState.Modified;
            _context.Entry(teamB).State = EntityState.Modified;
            _context.SaveChanges();

            if (gamematch.LeagueId == null) return Task.CompletedTask;

            var nextRound = gamematch.ParticipatingTeams.Where(x => x.Id == teamA.Id).First().Round;
            nextRound--;
            if (nextRound == 0)
            {

                var league = _context.Leagues.Where(x => x.Id == gamematch.LeagueId).First();
                league.LeagueState = LeagueState.Finished;
                _context.Entry(league).State = EntityState.Modified;
                _context.SaveChanges();

                var winner = gamematch.ParticipatingTeams.Where(x => x.Result == Result.Winner).First().Team;
                winner.IsAvailable = true;
                _context.Entry(winner).State = EntityState.Modified;
                _context.SaveChanges();
                return Task.CompletedTask;
            }

            //takes lowest int of Seeds
            var winnerSeed = Math.Min(Convert.ToByte(teamA.Seed), Convert.ToByte(teamB.Seed));
            var nextMatchTeam = _context.MatchTeams.Where(x => x.Round == nextRound).Where(x => x.Seed == winnerSeed).First();
            //adding winning team
            nextMatchTeam.Team = gamematch.ParticipatingTeams.Where(x => x.Result == Result.Winner).First().Team;
            //updating to sql
            _context.Entry(nextMatchTeam.Team).State = EntityState.Modified;
            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
