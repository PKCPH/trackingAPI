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

            while (_context.Teams.Count(x => (bool)x.IsAvailable) > 1)
            {
                MatchController matchController = new(_context);
                TeamPicker teamPicker = new();
                await matchController.Create(teamPicker);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task FindAndPlayMatches()
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
        /*        await PlayGameMatch(firstGameMatch);*/
                Thread thread = new Thread(async () => { await PlayGameMatch(firstGameMatch); });
                thread.Start();
                //Console.WriteLine($"*********THREAD #{thread.ManagedThreadId} for MATCH {firstGameMatch.Id} is started");
                Thread.Sleep(100);
            }
        }
        //return Task.CompletedTask;
    }

    public IOrderedEnumerable<GameMatch> GetListOfScheduledGameMatchesByDateTime()
    {
        List<GameMatch> gameMatches = new List<GameMatch>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            foreach (var match in _context.Matches.Where(x => x.MatchState == MatchState.NotStarted))
            {
                var match2 = AddTeamsToParticipatingTeams(match, match.Id);
                gameMatches.Add(match2);
            }
        }
        var gameMatchesSortByOrder = gameMatches.OrderBy(x => x.DateOfMatch);

        return gameMatchesSortByOrder;
    }

    public GameMatch AddTeamsToParticipatingTeams(GameMatch gameMatch, Guid matchId)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            var matches = _context.MatchTeams.Where(x => x.MatchId == matchId).Include(x => x.Team).ToList();
            gameMatch.ParticipatingTeams.Add(matches.First());
            gameMatch.ParticipatingTeams.Add(matches.Last());
        }
        return gameMatch;

    }

    public async Task PlayGameMatch(GameMatch gameMatch)
    {
        Random random = new Random();
        List<MatchTeam> matchTeams = new List<MatchTeam>();
        LiveMatchBackgroundTask liveMatchBackgroundTask = new(_services);
        CancellationToken stoppingToken;


        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            liveMatchBackgroundTask.ExecuteLiveMatch(gameMatch);
             BetsHandler betsHandler = new(_services);
            gameMatch.MatchState = MatchState.Finished;
         

                if (gameMatch.MatchState == MatchState.Finished)
                {
                    await betsHandler.UpdateBalancesOnMatchFinish(gameMatch);
                }

            // detach the gameMatch instance to avoid conflicts with the context
            _context.Entry(gameMatch).State = EntityState.Detached;

            // attach the updated gameMatch instance to the context and save changes
            _context.Matches.Update(gameMatch);
            await _context.SaveChangesAsync();

 /*           _context.Entry(gameMatch).State = EntityState.Modified;*/

            foreach (var item in _context.MatchTeams)
            {
                matchTeams.Add(item);
            }
            //foreach matchTeams where MatchId is matching the selected gameMatch.Id
            foreach (var item2 in matchTeams.Where(x => x.MatchId == gameMatch.Id))
            {
                //foreach team.id that is matching with matchTeams.teamId
                foreach (var item3 in _context.Teams.Where(x => x.Id == item2.Team.Id)) item3.IsAvailable = true;
            }
            _context.SaveChanges();
        }
    }
}
