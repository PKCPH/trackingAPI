using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public class OddsHandler
    {
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
                winChances[i] = expectedScores[i] / sum;
                winOdds[i] = 1 / winChances[i];
                winChances[i] *= 100;
            }

            Dictionary<Team, Tuple<double, double>> result = new Dictionary<Team, Tuple<double, double>>();
            for (int i = 0; i < n; i++)
            {
                result.Add(teams[i], new Tuple<double, double>(Math.Round(winChances[i], 2), Math.Round(winOdds[i], 2)));
            }

            return result;
        }

        public float CalculateComboOdds(params double[] oddsList)
        {
            float combinedOdds = 1;

            for (int i = 0; i < oddsList.Length; i++)
            {
                combinedOdds *= (float)oddsList[i];
            }
            Math.Round(combinedOdds, 2);
            return combinedOdds;
        }

        public double AddDrawChances(double[] rating)
        {
            int drawChances = 0;



            return drawChances;
        }
    }
}
