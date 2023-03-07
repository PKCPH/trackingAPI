﻿using System.Diagnostics;
using System.Timers;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using trackingAPI.Helpers;

namespace trackingAPI.BackgroundHelpers;


public class LiveMatchBackgroundTask
{
    private readonly IServiceProvider _services;
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

        while (timer.Elapsed.TotalSeconds < BackgroundTaskConfiguration.MatchLengthInSeconds)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");

            IsGoalScoredChance(gameMatch);
            Console.WriteLine($"Match: {gameMatch.Id} Time: {fromTimer}");
            Thread.Sleep(1000);
        }
        timer.Stop();

        var teamA = gameMatch.ParticipatingTeams.First();
        var teamB = gameMatch.ParticipatingTeams.Last();
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            if (teamA.TeamScore > teamB.TeamScore) { teamA.Result = Result.Winner; teamB.Result = Result.Loser; }
            if (teamA.TeamScore < teamB.TeamScore) { teamA.Result = Result.Loser; teamB.Result = Result.Winner; }
            if (teamA.TeamScore == teamB.TeamScore) { teamA.Result = Result.Draw; teamB.Result = Result.Draw; }

            _context.Entry(teamA).State = EntityState.Modified;
            _context.Entry(teamB).State = EntityState.Modified;

            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }

    public GameMatch IsGoalScoredChance(GameMatch gameMatch)
    {
        Random rnd = new Random();
        var ballPossessionNumber = rnd.Next(100);
        bool goalToTeamA = false;
        var chanceOfGoal = rnd.Next(1, 100);
        if (ballPossessionNumber < 50) goalToTeamA = true;
        if (chanceOfGoal > 2) return gameMatch;
        Console.WriteLine($"GOAL IS SCORED");
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                   .GetRequiredService<DatabaseContext>();

            //if teamA is scoring then Teamscore++ other the same for teamB
            if (goalToTeamA)
            {
                gameMatch.ParticipatingTeams.First().TeamScore++;
                _context.Entry(gameMatch.ParticipatingTeams.First()).State = EntityState.Modified;
            }
            else
            {
                gameMatch.ParticipatingTeams.Last().TeamScore++;
                _context.Entry(gameMatch.ParticipatingTeams.Last()).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }
        return gameMatch;
    }
}

