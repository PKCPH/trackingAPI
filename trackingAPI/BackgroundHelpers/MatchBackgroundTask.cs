using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Configurations;
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
    public async Task CreateNewMatchesFromAvailableTeams()
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
                Console.WriteLine($"Number of teams that are available: {_context.Teams.Count(x => (bool)x.IsAvailable)}");
                await Task.Delay(5000);
            }
        }
    }

    public async Task FindAndPlayMatches(CancellationToken stoppingToken, PeriodicTimer timer)
    {
        var gameMatches = ScheduledTaskOfTodaysMatches().OrderBy(x => x.DateOfMatch);
        var firstMatch = gameMatches.First();
        var matchTeams = ListOfMatchTeams();

        DateTime now = DateTime.Now;
        Console.WriteLine("FindAndPlayMatches");
        //// If now has past the schedule time of the match  
        if (now > firstMatch.DateOfMatch)
        {
            TriggerScheduledMatches(firstMatch, matchTeams);
        }

        //int msUntilMatchStarts = (int)((TimeOfMatch - now).TotalMilliseconds);

        //// Set the timer to elapse only once, at 4:00.
        ////t.change(msUntilMatchStarts, Timeout.Infinite);



    }

    public List<MatchTeam> ListOfMatchTeams()
    {
        List<MatchTeam> matchTeams = new List<MatchTeam>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            foreach (var item in _context.MatchTeams)
            {
                matchTeams.Add(item);
            }
        }
        //var gameMatchesSortByOrder = matchTeams.OrderBy(x => x.DateOfMatch);

        Console.WriteLine("ScheduledTaskOfTodaysMatches");
        return matchTeams.ToList();
    }

    public IOrderedEnumerable<GameMatch> ScheduledTaskOfTodaysMatches()
    {
        List<GameMatch> gameMatches = new List<GameMatch>();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            foreach (var item in _context.Matches.Where(x => x.MatchState == MatchState.NotStarted))
            {
                gameMatches.Add(item);
            }
        }
        var gameMatchesSortByOrder = gameMatches.OrderBy(x => x.DateOfMatch);

        Console.WriteLine("ScheduledTaskOfTodaysMatches");
        return gameMatchesSortByOrder;
    }

    public void TriggerScheduledMatches(GameMatch gameMatch, List<MatchTeam> matchTeam)
    {
        Random random = new Random();

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            foreach (var item in _context.Matches.Where(x => x.Id == gameMatch.Id))
            {
                item.TeamAScore = random.Next(0, 3);
                item.TeamBScore = random.Next(0, 3);
                
            }
            //sets IsAvaible back to true, but should be specific...
            foreach(var item in _context.Teams.Where(x => x.IsAvailable == false))
            {
                item.IsAvailable = true;
            }

            _context.SaveChanges();
        }
        Console.WriteLine("TriggerScheduledMatches");
    }
}
