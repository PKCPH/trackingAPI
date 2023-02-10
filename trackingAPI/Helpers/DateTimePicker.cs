using Newtonsoft.Json;
using trackingAPI.Configurations;
using static trackingAPI.Configurations.BackgroundTaskConfiguration;

namespace trackingAPI.Helpers
{
    public static class DateTimePicker
    {
        public static DateTime CreateRandomMatchTime()
        {
            var rnd = new Random();
            var date = DateTimeStartingPoint;

            //Timespan of the random scheduled time 
            var minutes = rnd.Next(0, ScheduledTimeSpanInMinutes);
            //when the match starts the earliest on a given day
            var timeOfDayHours = TimeSpan.FromHours(StartHourOfScheduledTimeSpan);
            timeOfDayHours += TimeSpan.FromMinutes(minutes);
            var pickedDateTime = date + timeOfDayHours;
       
            //if pickedDateTime is before CustomLocalTime.TimeOfDay return with +1 day else return
            return (Convert.ToDateTime(CustomLocalTime) > pickedDateTime) ? pickedDateTime.AddDays(1) : pickedDateTime;

        }
    }
}
