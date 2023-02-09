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
        //// If now has past the schedule time of the match  
        if (now > firstMatch.DateOfMatch)
        {
            TriggerScheduledMatches(firstMatch, matchTeams);
        }
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

            var matchTeams = ListOfMatchTeams();

            foreach (var item in _context.Matches.Where(x => x.Id == gameMatch.Id))
            {
                item.TeamAScore = random.Next(0, 3);
                item.TeamBScore = random.Next(0, 3);
                item.MatchState = MatchState.Finished;
            }

            //foreach matchTeams where MatchId is matching the selected gameMatch.Id
            foreach (var item2 in matchTeams.Where(x => x.MatchId == gameMatch.Id))
            {
                //foreach team.id that is matching with matchTeams.teamId
                foreach (var item3 in _context.Teams.Where(x => x.Id == item2.TeamId)) item3.IsAvailable = true;
            }
            _context.SaveChanges();
        }
    }
}
