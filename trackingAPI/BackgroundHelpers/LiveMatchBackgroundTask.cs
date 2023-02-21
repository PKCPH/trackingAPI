using System.Diagnostics;
using System.Timers;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace trackingAPI.BackgroundHelpers;


public class LiveMatchBackgroundTask
{
    private readonly IServiceProvider _services;
    public int TeamAScore;
    public int TeamBScore;

    public LiveMatchBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }

    public Task ExecuteLiveMatch(GameMatch gameMatch)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            gameMatch.MatchState = MatchState.Playing;

            _context.Entry(gameMatch).State = EntityState.Modified;
            _context.SaveChanges();
        }

        Stopwatch timer = new Stopwatch();
        timer.Start();

        while (timer.Elapsed.TotalSeconds < 60)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");

            IsGoalScoredChance(gameMatch);
            Console.WriteLine($"Match: {gameMatch.Id} Time: {fromTimer}");

            Console.WriteLine();

            Thread.Sleep(1000);
        }
        timer.Stop();


        return Task.CompletedTask;
    }

    public void IsGoalScoredChance(GameMatch gameMatch)
    {
        Random rnd = new Random();
        var ballPossessionTeam = rnd.Next(100);
        bool GoalToTeamA = false;
        var chanceOfGoal = rnd.Next(1, 100);
        if (ballPossessionTeam < 50) GoalToTeamA = true;
        if (chanceOfGoal > 2) return;

        Console.WriteLine($"GOAL IS SCORED");
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

            if (GoalToTeamA) gameMatch.TeamAScore++; else gameMatch.TeamBScore++;

            _context.Entry(gameMatch).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}

