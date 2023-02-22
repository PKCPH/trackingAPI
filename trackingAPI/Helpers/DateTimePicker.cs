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
            var date = DateTime.Today; //.today when not testing
            DateTime dateTime = DateTime.UtcNow;

            //Timespan of the random scheduled time 
            var minutes = rnd.Next(0, ScheduledTimeSpanInMinutes);
            //when the match starts the earliest on a given day
            var timeOfDayHours = TimeSpan.FromHours(StartHourOfScheduledTimeSpan);
            timeOfDayHours += TimeSpan.FromMinutes(minutes);
            var pickedDateTime = date + timeOfDayHours;

            //if pickedDateTime is before datetime return it with +1 day else return its normal datetime
            return (pickedDateTime < dateTime) ? pickedDateTime.AddDays(1) : pickedDateTime;

            //master merge
        }
    }
}
