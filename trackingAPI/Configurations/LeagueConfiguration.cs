namespace trackingAPI.Configurations
{
    public static class LeagueConfiguration
    {
        //How long interval between matches should be set
        //public static DateTime IntervalBetweenMatches => IntervalBetweenMatches.AddSeconds(550);

        public static int AmountOfTeams = 32;
        public static int IntervalBetweenMatchesIMinutes { get; set; } = 2;
    }
}
