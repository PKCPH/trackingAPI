using trackingAPI.Data;
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

        //var matches = GenerateTournamentMatches(league);

        Console.WriteLine();
        //BracketHelper.Play(randomizedTeams);

        //CreateFirstRound(0, randomizedTeams);
        //adding the first round to league
        //league.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
        Console.WriteLine();
        //CreateOtherRounds(league, rounds);
    }

    //private static void CreateOtherRounds(League league, int rounds)
    //{
    //    int round = 2;
    //    List<MatchupModel> previousRound = league.Rounds[0];
    //    List<MatchupModel> currentRound = new List<MatchupModel>();
    //    MatchupModel currentMatchup = new MatchupModel();

    //    while (round <= rounds)
    //    {
    //        //adding the current round
    //        foreach (MatchupModel match in previousRound)
    //        {
    //            currentMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });
    //            if (currentMatchup.Entries.Count > 1)
    //            {
    //                currentMatchup.MatchupRound = round;
    //                currentRound.Add(currentMatchup);
    //                currentMatchup = new MatchupModel();
    //            }
    //        }

    //        //adding every entry to league.rounds and setting previousRound
    //        league.Rounds.Add(new Round { Matches = currentRoundMatches });
    //        previousRound = currentRound;

    //        currentRound = new List<MatchupModel>();
    //        round += 1;
    //    }
    //}

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

        while(val < teamCount)
        {
            // output = output + 1;
            output += 1;

            // val = val * 2;
            val *= 2;
        }
        return output;
    }
    private static List<Team> RandomizeTeamOrder(List<LeagueTeam> teams)
    {
        //return teams.OrderBy(x => Guid.NewGuid()).ToList();
        return (List<Team>)teams.OrderBy(x => Guid.NewGuid())
            .Select(x => x.Team).ToList();
    }
}
