using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public class OddsHandler
    {

        public Dictionary<Team, double> WinChances(params Team[] teams)
        {
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
                expectedScores[i] = Math.Pow(10, ratings[i] / 400);
                sum += expectedScores[i];
            }

            double[] winChances = new double[n];
            for (int i = 0; i < n; i++)
            {
                winChances[i] = expectedScores[i] / sum;
            }

            Dictionary<Team, double> result = new Dictionary<Team, double>();
            for (int i = 0; i < n; i++)
            {
                result.Add(teams[i], winChances[i]);
            }

            return result;
        }

        //public Dictionary<Team, Tuple<double, double>> WinOdds(Dictionary<Team, double> winChances)
        //{
        //    Dictionary<Team, Tuple<double, double>> result = new Dictionary<Team, Tuple<double, double>>();

        //    foreach (KeyValuePair<Team, double> kvp in winChances)
        //    {
        //        double odds = 1 / kvp.Value;
        //        result.Add(kvp.Key, odds);
        //    }

        //    return result;
        //}

        public Dictionary<Team, Tuple<double, double>> WinChancesAndOdds(params Team[] teams)
        {
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
                expectedScores[i] = Math.Pow(10, Convert.ToDouble(ratings[i] / 400))
                    ;
                sum += expectedScores[i];
                Convert.ToDecimal(expectedScores[i]);
            }

            double[] winChances = new double[n];
            double[] winOdds = new double[n];
            for (int i = 0; i < n; i++)
            {
                winChances[i] =(expectedScores[i] / sum);
                if (winChances[i] < 0.5)
                {
                    winOdds[i] = Math.Round((1.0 / (1.0 - winChances[i])), 2);
                }
                else
                {
                    winOdds[i] = Math.Round((1.0 / winChances[i]), 2);
                }
                //winChances[i] = Convert.ToDecimal(expectedScores[i] / sum);
                //winOdds[i] = 1 / winChances[i];
            }

            Dictionary<Team, Tuple<double, double>> result = new Dictionary<Team, Tuple<double, double>>();
            for (int i = 0; i < n; i++)
            {
                result.Add(teams[i], new Tuple<double, double>(winChances[i], winOdds[i]));
            }

            return result;
        }

        //public Dictionary<Team, Tuple<Decimal, double>> WinChancesAndOdds(params Team[] teams)
        //{
        //    int n = teams.Length;
        //    double[] ratings = new double[n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        ratings[i] = (double)teams[i].Rating;
        //    }

        //    double[] expectedScores = new double[n];
        //    double sum = 0;
        //    for (int i = 0; i < n; i++)
        //    {
        //        expectedScores[i] = Math.Pow(10, ratings[i] / 400);
        //        sum += expectedScores[i];
        //    }

        //    double[] winChances = new double[n];
        //    double[] winOdds = new double[n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        winChances[i] = expectedScores[i] / sum;
        //        winOdds[i] = 1 / Math.Pow(winChances[i], 1.5);
        //    }

        //    // Sort the teams by their win chances in descending order
        //    var sortedTeams = teams.OrderByDescending(t => winChances[Array.IndexOf(teams, t)]);

        //    Dictionary<Team, Tuple<double, double>> result = new Dictionary<Team, Tuple<double, double>>();
        //    foreach (var team in sortedTeams)
        //    {
        //        int index = Array.IndexOf(teams, team);
        //        result.Add(team, new Tuple<double, double>(winChances[index], winOdds[index]));
        //    }

        //    return result;
        //}
    }
}
