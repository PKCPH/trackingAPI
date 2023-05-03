using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;


public class LiveMatchBackgroundTask
{
    private readonly IServiceProvider _services;
    private double participantSuperiority;
    public LiveMatchBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }
    public Task ExecuteLiveMatch(ref Gamematch gamematch)
    {
        //used for IsGoalScored() to calculate elo rating advantages
        this.participantSuperiority = ParticipantSuperiority(
            Convert.ToDouble(gamematch.ParticipatingTeams.First().Team.Rating),
            Convert.ToDouble(gamematch.ParticipatingTeams.Last().Team.Rating)) * 100;

        //First Half
        PlayGameHalf(gamematch, MatchState.FirstHalf);

        //Half Time
        UpdateMatchState(gamematch, MatchState.HalfTimePause);
        Thread.Sleep(LiveGamematchConfiguration.HalfTimeBreakLengthInMilliSeconds);
        
        //Second Half
        PlayGameHalf(gamematch, MatchState.SecondHalf);
     
        //Overtime
        if (gamematch.ParticipatingTeams.First().TeamScore != gamematch.ParticipatingTeams.Last().TeamScore
            || gamematch.IsDrawAllowed) return Task.CompletedTask;
        PlayOvertime(gamematch, MatchState.OverTime);
     
        //Penalty Shootout
        if (gamematch.ParticipatingTeams.First().TeamScore != gamematch.ParticipatingTeams.Last().TeamScore) return Task.CompletedTask;
        PlayPenaltyShootout(gamematch, MatchState.PenaltyShootOut);

        return Task.CompletedTask;
    }


    public Gamematch PlayGameHalf(Gamematch gamematch, MatchState matchState)
    {
        Stopwatch timer = new Stopwatch();
        UpdateMatchState(gamematch, matchState);
        timer.Start();
        
        while (timer.Elapsed.TotalSeconds < LiveGamematchConfiguration.GamematchLengthInSeconds)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");
            Console.WriteLine($"Match: {gamematch.Id} Time: {fromTimer} PlayingState: {gamematch.MatchState}");
            IsGoalScoredChance(gamematch);
            Thread.Sleep(1000);
        }
        timer.Stop();

        return gamematch;
    }
    
    //overtime and penalty shootout is only called is draw is not allowed
    public Gamematch PlayOvertime(Gamematch gamematch, MatchState matchState)
    {
        Stopwatch timer = new Stopwatch();
        UpdateMatchState(gamematch, matchState);
        timer.Start();

        while (timer.Elapsed.TotalSeconds < LiveGamematchConfiguration.OvertimeLengthInSeconds)
        {
            TimeSpan result = TimeSpan.FromSeconds(timer.Elapsed.TotalSeconds);
            string fromTimer = result.ToString("mm':'ss");

            IsGoalScoredChance(gamematch);
            Console.WriteLine($"Match overtime: {gamematch.Id} Time: {fromTimer}");
            Console.WriteLine();
            Thread.Sleep(1000);
        }
        timer.Stop();
        return gamematch;
    }

    public Gamematch PlayPenaltyShootout(Gamematch gamematch, MatchState matchState)
    {
        Stopwatch timer = new Stopwatch();
        Random rnd = new Random();
        var rounds = 0;
        var teamAPKScore = 0;
        var teamBPKScore = 0;

        UpdateMatchState(gamematch, matchState);
        while (teamAPKScore == teamBPKScore || rounds < 4)
        {
            PenaltyKick(ref teamAPKScore, gamematch.ParticipatingTeams.First(), rnd);
            Console.WriteLine($"PK SCORE: {gamematch.ParticipatingTeams.First().Team.Name} - {teamAPKScore} VS {gamematch.ParticipatingTeams.Last().Team.Name} - {teamBPKScore}");
            Thread.Sleep(1000);
            PenaltyKick(ref teamBPKScore, gamematch.ParticipatingTeams.Last(), rnd);
            Console.WriteLine($"PK SCORE: {gamematch.ParticipatingTeams.First().Team.Name} - {teamAPKScore} VS {gamematch.ParticipatingTeams.Last().Team.Name} - {teamBPKScore}");
            rounds++;
            Thread.Sleep(1000);
        }
        return gamematch;
    }

    public Task PenaltyKick(ref int teamPKScore, MatchTeam team, Random rnd)
    {
        Thread.Sleep(LiveGamematchConfiguration.PenaltyShootoutTimeIntervalMilliInSeconds);
        int chance = rnd.Next(1, 100);
        if (chance > 95)
        {
            teamPKScore++;

            using (var scope = _services.CreateScope())
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<DatabaseContext>();
                team.TeamScore++;
                _context.Entry(team).State = EntityState.Modified;
            }
        }
        return Task.CompletedTask;
    }

    //Every second this is called to determine who will have the
    //ballPossession and with a small chance of the goal to be scored
    public Gamematch IsGoalScoredChance(Gamematch gameMatch)
    {
        Random rnd = new Random();
        var ballPossessionTeam = rnd.Next(10000)/100;
        bool GoalToTeamA = false;
        var chanceOfGoal = rnd.Next(1, 200);
        if (ballPossessionTeam > 100 - this.participantSuperiority) GoalToTeamA = true;
        if (chanceOfGoal > 1) return gameMatch;

        Console.WriteLine($"GOAL IS SCORED");
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            //if teamA is scoring then Teamscore++ other the same for teamB
            if (GoalToTeamA)
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

    //Calculates the advantage that Team A has over Team B using a modified version of Dr. Elos chess rating equations
    public static double ParticipantSuperiority(double participantA, double participantB)
    {
        double participantSuperiority = (1 / (1 + Math.Pow(10, Convert.ToDouble(participantB - participantA) / 20)));
        return participantSuperiority;
    }

    public Task UpdateMatchState(Gamematch gamematch, MatchState matchState)
    {
        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            gamematch.MatchState = matchState;
            _context.Entry(gamematch).State = EntityState.Modified;
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }
}

