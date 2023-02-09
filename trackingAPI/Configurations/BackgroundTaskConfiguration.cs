namespace trackingAPI.Configurations;

public static class BackgroundTaskConfiguration
{
    public static bool IsEnabled { get; set; } = true;

    public static int StartOfMatchTimeOfDay { get; set; } = 13;

    public static int RandomMatchScheduleTimeSpanInHours { get; set; } = 1;

    public static TimeSpan BackgroundTaskTimerTickTimespan { get; set; } = TimeSpan.FromSeconds(60);
}
