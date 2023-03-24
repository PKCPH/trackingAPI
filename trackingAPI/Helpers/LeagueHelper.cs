using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueHelper
{
    public League CreateRounds(League league, DatabaseContext _context)
    {
        if (league.Teams.Count == 0) league.Teams = GetListOfTeams(league, _context).Teams.ToList();

        league.LeagueState = LeagueState.NotStarted;
        var teams = league.Teams.Select(x => x.Team).ToList();

        int rounds = FindNumberOfRounds(teams.Count);
        int byes = NumberOfByes(rounds, teams.Count);

        var gamematches = CreateGamematchRounds(rounds, byes, teams, league.StartDate);
        league.Gamematches = gamematches;

        return league;
    }

    private List<Gamematch> CreateGamematchRounds(int rounds, int byes, List<Team> teams, DateTime leagueDateTime)
    {
        //Creating first round
        List<Gamematch> gamematches = new List<Gamematch>();
        Random rnd = new();
        var maxTeamCount = teams.Count + byes;
        var teamLoopCount = maxTeamCount / 2;
        var roundTerm = GetRoundTerm(rounds);

        for (int i = 1; i <= teamLoopCount; i++)
        {
            Gamematch gamematch = new()
            {
                ParticipatingTeams = new List<MatchTeam>(),
                DateOfMatch = leagueDateTime,
                IsDrawAllowed = false,
                RoundTerm= roundTerm,

            };
            leagueDateTime = leagueDateTime.AddMinutes(LeagueConfiguration.IntervalBetweenMatchesIMinutes);
            var availableTeams = teams.Where(x => (bool)x.IsAvailable).ToList();
            MatchTeam matchTeamA;
            MatchTeam matchTeamB;

            if (byes > 0)
            {
                Team byeTeam = new Team { Id = Guid.NewGuid(), Name = "BYE", IsAvailable = false };
                var teamA = availableTeams.OrderBy(x => rnd.Next()).Take(1).ToList().First();
                teamA.IsAvailable = false;

                matchTeamA = new MatchTeam { Team = teamA, Seed = i, Round = rounds };
                matchTeamB = new MatchTeam { Team = byeTeam, Seed = maxTeamCount, Round = rounds };
                byes--;
            }
            else
            {
                var twoRandomAvailableTeams = availableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();
                var teamA = twoRandomAvailableTeams.First();
                var teamB = twoRandomAvailableTeams.Last();
                teamA.IsAvailable = false;
                teamB.IsAvailable = false;

                matchTeamA = new MatchTeam { Team = teamA, Seed = i, Round = rounds };
                matchTeamB = new MatchTeam { Team = teamB, Seed = maxTeamCount, Round = rounds };
            }
            maxTeamCount--;
            gamematch.ParticipatingTeams.Add(matchTeamA);
            gamematch.ParticipatingTeams.Add(matchTeamB);
            gamematches.Add(gamematch);
        }
        rounds--;
        //Creating rest of the rounds
        while (rounds > 0)
        {
            var maxTeamCount1 = maxTeamCount;
            maxTeamCount /= 2;
            roundTerm = GetRoundTerm(rounds);
            for (int i = 1; i <= maxTeamCount; i++)
            {
                Gamematch gamematch = new()
                {
                    ParticipatingTeams = new List<MatchTeam>(),
                    DateOfMatch = leagueDateTime,
                    IsDrawAllowed = false,
                    RoundTerm = roundTerm,
                };

                leagueDateTime = leagueDateTime.AddMinutes(LeagueConfiguration.IntervalBetweenMatchesIMinutes);
                MatchTeam matchTeamA = new MatchTeam { Team = null, Seed = i, Round = rounds };
                MatchTeam matchTeamB = new MatchTeam { Team = null, Seed = maxTeamCount1, Round = rounds };
                maxTeamCount1--;
                gamematch.ParticipatingTeams.Add(matchTeamA);
                gamematch.ParticipatingTeams.Add(matchTeamB);
                gamematches.Add(gamematch);
            }
            rounds--;
        }
        return gamematches;
    }

    public string GetRoundTerm(int round)
    {
        if (round == 1) return "Grand Finale";
        else if (round == 2) return "Semi-Finale";
        else if (round == 3) return "Quarter-Finale";
        else
        {
            var output = 4;
            for (var i = 4; i <= round; i++)
            {
                output *= 2;
            }
            return $"1/{output} Finale";
        }
    }

    public League GetListOfTeams(League league, DatabaseContext _context)
    {
        //Random rnd = new Random();
        var availableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).Take(LeagueConfiguration.AmountOfTeams);
        foreach (var team in availableTeams)
        {
            LeagueTeam leagueTeamA = new LeagueTeam { Team = team };
            league.Teams.Add(leagueTeamA);
        }
        return league;
    }
    public static int NumberOfByes(int rounds, int numberOfTeams)
    {
        int totalTeams = 1;

        for (int i = 1; i <= rounds; i++) { totalTeams *= 2; }

        int byes = totalTeams - numberOfTeams;
        return byes;
    }

    private static int FindNumberOfRounds(int teamCount)
    {
        int rounds = 1;
        int val = 2;

        while (val < teamCount)
        {
            rounds += 1;
            val *= 2;
        }
        return rounds;
    }

    public static Dictionary<Team, double> TournamentWinChances(params Team[] teams)
    {
        //How harshly the algorithm judges a team based on their rating. the lower the value the harsher the judgement. Any values below 0 gives faulty answers
        double sensisivity = 20;
        Dictionary<Team, double> result = new Dictionary<Team, double>();

        int n = teams.Length;
        double[] ratings = new double[n];
        for (int i = 0; i < n; i++)
        {
            ratings[i] = (double)teams[i].Rating;
        }

        double[] expectedScores = new double[n];
        double sum = 0;
        for (int i = 0; i < n; i++)
        {
            expectedScores[i] = Math.Pow(10, ratings[i] / sensisivity);
            sum += expectedScores[i];
        }

        double[] winChances = new double[n];
        for (int i = 0; i < n; i++)
        {
            winChances[i] = expectedScores[i] / sum;
        }

        for (int i = 0; i < n; i++)
        {
            result.Add(teams[i], winChances[i]);
        }

        return result;
    }
}
