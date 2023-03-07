namespace trackingAPI.Configurations;

public static class BackgroundTaskConfiguration
{
    //public static bool IsEnabled { get; set; } = true;

    ////The timespan in hours of the random scheduled time of a match //use x*60 for hours.
    //public static int ScheduledTimeSpanInMinutes { get; set; } = 8 * 60;

    ////Determine the earliest time of the day for the ScheduledTimeSpanInHours
    //public static int StartHourOfScheduledTimeSpan { get; set; } = 12;

    ////Datetime of which day the match is playing at (for now the same day as created)
    ////using DateTime.Now for testing, but use DateTime.Today when not testing

    ////How often the backgroundtask is checking for changes.
    //public static TimeSpan TimerTickTimeSpan { get; set; } = TimeSpan.FromSeconds(60);



    ///////////////////////TESTING/////////////////////////////////
    /////
    public static bool IsEnabled { get; set; } = true;

    //The timespan in hours of the random scheduled time of a match //use x*60 for hours.
    public static int ScheduledTimeSpanInMinutes { get; set; } = 5 + 1; //default is 8*60, testing is 5

    //Determine the earliest time of the day for the ScheduledTimeSpanInHours
    public static int StartHourOfScheduledTimeSpan { get; set; } = 0; //default is 12, testing is 0

    //How often the backgroundtask is checking for changes.
    public static TimeSpan TimerTickTimeSpan { get; set; } = TimeSpan.FromSeconds(6); //default is 60, testing is 6

    public static int MatchLengthInSeconds { get; set; } = 20; //default is 5400, testing is 20

    public static int DaysAddedThanNow { get; set; } = 0; //default is 1, testing is 0

    ///fkkkkk
}
