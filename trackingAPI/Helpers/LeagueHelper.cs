using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueHelper
{
    public League CreateRounds(League league, DatabaseContext _context)
    {
        LeagueHelper leagueHelper = new LeagueHelper();
        ////testing
        //var leagueTeams = leagueHelper.GetListOfEightTeams(league, _context);
        //league.Teams = leagueTeams.Teams.ToList();

        league.LeagueState = LeagueState.NotStarted;

        var randomizedTeams = RandomizeTeamOrder(league.Teams.ToList());
        int rounds = FindNumberOfRounds(randomizedTeams.Count);
        //int byes = NumberOfByes(rounds, randomizedTeams.Count);

        var teams = randomizedTeams.Select(x => x.Team).ToList();
        var gamematches = CreateGamematchRounds(teams, league.StartDate);

        league.Gamematches = gamematches;
        return league;
    }

    private List<Gamematch> CreateGamematchRounds(List<Team> teams, DateTime leagueDateTime)
    {
        /////////////////FIRST ROUNDS///////////////////////////
        var roundsNumber = (int)Math.Log(teams.Count, 2);
        List<Gamematch> gamematches = new List<Gamematch>();
        Random rnd = new();
        var maxTeamCount = teams.Count;
        var teamLoopCount = teams.Count / 2;
        for (int i = 1; i <= teamLoopCount; i++)
        {
            Gamematch gamematch = new()
            {
                ParticipatingTeams = new List<MatchTeam>(),
                Round = roundsNumber,
                IsLeagueGame = true,
                DateOfMatch = leagueDateTime
            };

            leagueDateTime = leagueDateTime.AddDays(1);
            var TwoRandomAvailableTeams = teams.OrderBy(x => rnd.Next()).Take(2).ToList();

            var teamA = TwoRandomAvailableTeams.First();
            var teamB = TwoRandomAvailableTeams.Last();
            teamA.IsAvailable = false;
            teamB.IsAvailable = false;

            MatchTeam matchTeamA = new MatchTeam { Team = teamA, Seed = i };
            MatchTeam matchTeamB = new MatchTeam { Team = teamB, Seed = maxTeamCount };
            maxTeamCount--;

            gamematch.ParticipatingTeams.Add(matchTeamA);
            gamematch.ParticipatingTeams.Add(matchTeamB);
            gamematches.Add(gamematch);
        }
        roundsNumber--;
        ////////////////////////OTher rounds

        var teamsCount = teams.Count / 2;
        while (roundsNumber > 0)
        {
            var maxTeamCount1 = teamsCount;
            teamsCount /= 2;
            for (int i = 1; i <= teamsCount; i++)
            {
                Gamematch gamematch = new()
                {
                    ParticipatingTeams = new List<MatchTeam>(),
                    Round = roundsNumber,
                    IsLeagueGame = true,
                    DateOfMatch = leagueDateTime
                };

                leagueDateTime = leagueDateTime.AddDays(1);
                MatchTeam matchTeamA = new MatchTeam { Team = null, Seed = i };
                MatchTeam matchTeamB = new MatchTeam { Team = null, Seed = maxTeamCount1 };
                maxTeamCount1--;
                gamematch.ParticipatingTeams.Add(matchTeamA);
                gamematch.ParticipatingTeams.Add(matchTeamB);
                gamematches.Add(gamematch);
            }
            roundsNumber--;
        }
        return gamematches;
    }

    public League GetListOfEightTeams(League league, DatabaseContext _context)
    {
        Random rnd = new Random();

        var eightAvailableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList().Take(16);

        foreach (var team in eightAvailableTeams)
        {
            LeagueTeam leagueTeamA = new LeagueTeam { Team = team };
            league.Teams.Add(leagueTeamA);
        }
        return league;
    }

    private static int FindNumberOfRounds(int teamCount)
    {
        int output = 1;
        int val = 2;

        while (val < teamCount)
        {
            // output = output + 1;
            output += 1;

            // val = val * 2;
            val *= 2;
        }
        return output;
    }
    private static ICollection<LeagueTeam> RandomizeTeamOrder(List<LeagueTeam> teams)
    {
        return teams.OrderBy(x => Guid.NewGuid()).ToList();
    }
}
