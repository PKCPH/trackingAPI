namespace trackingAPI.Configurations
{
    public static class BackgroundTaskConfiguration
    {
        public static bool IsEnabled { get; set; } = true;

        public static int RandomMatchTimeTimeSpanInHours { get; set; } = 4;
    }
}
