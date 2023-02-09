using Newtonsoft.Json;
using trackingAPI.Configurations;

namespace trackingAPI.Helpers
{
    public static class DateTimePicker
    {
        public static DateTime CreateRandomMatchTime()
        {
            var rnd = new Random();
            //Timespan of the random scheduled time 
            var minutes = rnd.Next(0, BackgroundTaskConfiguration.RandomMatchScheduleTimeSpanInHours * 60);
            //when the match starts the earliest on a given day
            var timeOfDayHours = TimeSpan.FromHours(BackgroundTaskConfiguration.StartOfMatchTimeOfDay);
            timeOfDayHours += TimeSpan.FromMinutes(minutes);

            var dt = DateTime.Today + timeOfDayHours;

            return dt;
        }
    }
}
