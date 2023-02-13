namespace trackingAPI.Configurations
{
    public static class LiveMatchConfiguration
    {
        public static TimeSpan TimerTickTimeSpan { get; set; } = TimeSpan.FromSeconds(1);

        public static TimeSpan MatchLength { get; set; } = TimeSpan.FromMinutes(90);

    }
}
