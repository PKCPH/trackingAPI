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
        var leagueWithTeams = GetListOfEightTeams(league, _context);
        league.Teams = leagueWithTeams.Teams.ToList();

        league.LeagueState = LeagueState.NotStarted;
        var teams = league.Teams.Select(x => x.Team).ToList();

        int rounds = FindNumberOfRounds(teams.Count);
        int byes = NumberOfByes(rounds, teams.Count);
        try
        {
            var gamematches = CreateGamematchRounds(rounds, byes, teams, league.StartDate);
            league.Gamematches = gamematches;

        }
        catch (Exception)
        {

            throw;
        }

        return league;
    }

    public static int NumberOfByes(int rounds, int numberOfTeams)
    {
        int byes = 0;
        int totalTeams = 1;

        for (int i = 1; i <= rounds; i++)
        {
            totalTeams *= 2;
        }

        byes = totalTeams - numberOfTeams;

        return byes;
    }

    private List<Gamematch> CreateGamematchRounds(int rounds, int byes, List<Team> teams, DateTime leagueDateTime)
    {
        /////////////////FIRST ROUNDS///////////////////////////
        //var roundsNumber = (int)Math.Log(teams.Count, 2);
        List<Gamematch> gamematches = new List<Gamematch>();
        Random rnd = new();
        var maxTeamCount = teams.Count + byes;
        var teamLoopCount = maxTeamCount / 2;

        for (int i = 1; i <= teamLoopCount; i++)
        {
            Gamematch gamematch = new()
            {
                ParticipatingTeams = new List<MatchTeam>(),
                DateOfMatch = leagueDateTime,
                IsDrawAllowed = false
            };
            leagueDateTime = leagueDateTime.AddMinutes(3);
            var availableTeams = teams.Where(x => (bool)x.IsAvailable).ToList();
            
            if (byes > 0)
            {
                var twoRandomAvailableTeams = availableTeams.OrderBy(x => rnd.Next()).Take(1).ToList();
                var teamA = twoRandomAvailableTeams.First();
                teamA.IsAvailable = false;
                Team byeTeam = new Team { Id = Guid.NewGuid(), Name = "BYE", IsAvailable = false };

                MatchTeam matchTeamA = new MatchTeam { Team = teamA, Seed = i, Round = rounds };
                MatchTeam matchTeamB = new MatchTeam { Team = byeTeam, Seed = maxTeamCount, Round = rounds };
                maxTeamCount--;
                gamematch.ParticipatingTeams.Add(matchTeamA);
                gamematch.ParticipatingTeams.Add(matchTeamB);
                gamematches.Add(gamematch);
                byes--;
            }
            else
            {
                var twoRandomAvailableTeams = availableTeams.OrderBy(x => rnd.Next()).Take(2).ToList();
                var teamA = twoRandomAvailableTeams.First();
                var teamB = twoRandomAvailableTeams.Last();
                teamA.IsAvailable = false;
                teamB.IsAvailable = false;

                MatchTeam matchTeamA = new MatchTeam { Team = teamA, Seed = i, Round = rounds };
                MatchTeam matchTeamB = new MatchTeam { Team = teamB, Seed = maxTeamCount, Round = rounds };
                maxTeamCount--;
                gamematch.ParticipatingTeams.Add(matchTeamA);
                gamematch.ParticipatingTeams.Add(matchTeamB);
                gamematches.Add(gamematch);
            }
        }
        rounds--;
        ////////////////////////OTher rounds

        var teamsCount = maxTeamCount;
        while (rounds > 0)
        {
            var maxTeamCount1 = teamsCount;
            teamsCount /= 2;
            for (int i = 1; i <= teamsCount; i++)
            {
                Gamematch gamematch = new()
                {
                    ParticipatingTeams = new List<MatchTeam>(),
                    DateOfMatch = leagueDateTime,
                    IsDrawAllowed = false
                };

                leagueDateTime = leagueDateTime.AddMinutes(3);
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

    public League GetListOfEightTeams(League league, DatabaseContext _context)
    {
        Random rnd = new Random();

        var availableTeams = _context.Teams.Where(t => (bool)t.IsAvailable).ToList().Take(3);

        foreach (var team in availableTeams)
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
}
