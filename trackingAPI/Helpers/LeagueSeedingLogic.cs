﻿using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class LeagueSeedingLogic
{
    //Order list of teams randomly
    //check if list is big enough, if not add byes - 2*2*2*2 - 2^4
    //create first round of matchups
    //create the rest of the rounds
    public static void CreateRounds(League league)
    {
        MatchTeam matchTeam = new();
        var randomizedTeams = RandomizeTeamOrder(league.Teams.ToList());


        int rounds = FindNumberOfRounds(randomizedTeams.Count);
        //int byes = NumberOfByes(rounds, randomizedTeams.Count);

        //var matches = GenerateTournamentMatches(,league);
        Console.WriteLine();
        //BracketHelper.Play(randomizedTeams);

        var teams = randomizedTeams.Select(x => x.Team).ToList();

        //int[] array = new int[] { };


        var gamematches = CreateFirstRound(teams);

        //adding the first round to league
        //league.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
        Console.WriteLine();
        CreateOtherRounds(gamematches, gamematches.Count);
    }
    private static void CreateOtherRounds(List<Gamematch> gamematches, int rounds)
    {
    //    int round = 2;
    //    //List<MatchTeam> previousRound = league.
    //    //List<MatchupModel> currentRound = new List<MatchupModel>();
    //    //MatchupModel currentMatchup = new MatchupModel();

    //    var count = gamematches.Count;

    //    for (int i = 1; i < teams.Count; i++)
    //    {
    //        Gamematch gamematch = new()
    //        {
    //            ParticipatingTeams = new List<MatchTeam>()
    //        };

    //        var TwoRandomAvailableTeams = teams.OrderBy(x => rnd.Next()).Take(2).ToList();

    //        var teamA = TwoRandomAvailableTeams.First();
    //        var teamB = TwoRandomAvailableTeams.Last();

    //        MatchTeam matchTeamA = new MatchTeam { Team = teamA, Seed = i, Round = roundsNumber };
    //        i++;
    //        MatchTeam matchTeamB = new MatchTeam { Team = teamB, Seed = i, Round = roundsNumber };

    //        gamematch.ParticipatingTeams.Add(matchTeamA);
    //        gamematch.ParticipatingTeams.Add(matchTeamB);
    //        gamematch.DateOfMatch = DateTimePicker.CreateRandomMatchTime();

    //        roundsNumber--;

    //        gamematches.Add(gamematch);
    //    }

    }
    private static List<Gamematch> CreateFirstRound(List<Team> teams)
    {
        var roundsNumber = (int)Math.Log(teams.Count, 2);
        //var rounds = new Array[roundsNumber];
        List<Gamematch> gamematches = new List<Gamematch>();
        Random rnd = new();

        for (int i = 1; i < teams.Count; i++)
        {
            Gamematch gamematch = new()
            {
                ParticipatingTeams = new List<MatchTeam>()
            };

            var TwoRandomAvailableTeams = teams.OrderBy(x => rnd.Next()).Take(2).ToList();

            var teamA = TwoRandomAvailableTeams.First();
            var teamB = TwoRandomAvailableTeams.Last();

            MatchTeam matchTeamA = new MatchTeam { Team = teamA, Seed = i, Round = roundsNumber };
            i++;
            MatchTeam matchTeamB = new MatchTeam { Team = teamB, Seed = i, Round = roundsNumber };
           
                gamematch.ParticipatingTeams.Add(matchTeamA);
                gamematch.ParticipatingTeams.Add(matchTeamB);
                gamematch.DateOfMatch = DateTimePicker.CreateRandomMatchTime();
            
            roundsNumber--;

            gamematches.Add(gamematch);
        }

        return gamematches;
    }


    private static List<Gamematch> GenerateTournamentMatches(ICollection<MatchTeam> teams, League league)
    {
        List<Gamematch> matches = new List<Gamematch>();

        // Create initial matches with randomized team order
        var randomize3dTeams = teams.OrderBy(x => Guid.NewGuid()).ToList();

        //var randomize3dTeams = league.Teams.OrderBy(x => Guid.NewGuid()).ToList();

        //var randomizedTeams = league.Teams.ToList();

        //var rt = league.MatchLeagueRounds.OrderBy(x => Guid.NewGuid()).ToArray();

        for (int i = 0; i < randomize3dTeams.Count(); i += 2)
        {
            var match = new Gamematch
            {
                ParticipatingTeams = new List<MatchTeam> { randomize3dTeams[i], randomize3dTeams[i + 1] },
                MatchState = MatchState.NotStarted
            };
            matches.Add(match);
        }

        // Loop through each round and generate matches
        var round = 2;
        while (matches.Count > 1)
        {
            var roundMatches = new List<Gamematch>();
            for (int i = 0; i < matches.Count; i += 2)
            {
                var winner1 = matches[i].ParticipatingTeams.OrderBy(x => x.Seed).First();
                var winner2 = matches[i + 1].ParticipatingTeams.OrderBy(x => x.Seed).First();

                var match = new Gamematch
                {
                    ParticipatingTeams = new List<MatchTeam> { winner1, winner2 },
                    MatchState = MatchState.NotStarted,
                    TeamASeed = (int)winner1.Seed,
                    TeamBSeed = (int)winner2.Seed
                };
                roundMatches.Add(match);
            }

            matches = roundMatches;
            round++;
        }

        return matches;
    }



    //private static List<MatchupModel> CreateFirstRound(int byes, List<Team> teams)
    //{
    //    List<MatchupModel> output = new List<MatchupModel>();
    //    MatchupModel curr = new MatchupModel();

    //    foreach (Team team in teams)
    //    {
    //        curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });
    //        if (byes > 0 || curr.Entries.Count > 1)
    //        {
    //            curr.MatchupRound = 1;
    //            output.Add(curr);
    //            curr = new MatchupModel();
    //            if (byes > 0)
    //            {
    //                byes -= 1;
    //            }
    //        }

    //    }
    //    return output;
    //}

    //private static int NumberOfByes(int rounds, int numberOfTeams)
    //{
    //    int output = 0;
    //    int totalTeams = 1;

    //    for (int i = 1; i < rounds; i++)
    //    {
    //        totalTeams *= 2;
    //    }
    //    output = totalTeams - numberOfTeams;
    //    return output;
    //}

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
        //return (teams.OrderBy(x => Guid.NewGuid())
        //    .Select(x => x.Team);
    }
}
