using Newtonsoft.Json;

namespace trackingAPI.Helpers
{
    public static class DateTimePicker
    {
        public static DateTime CreateRandomMatchTime()
        {
            var rnd = new Random();//Fixed seed, just termporarily
            var minutes = rnd.Next(0, 6 * 60);
            var timeOfDayHours = TimeSpan.FromHours(14);
            timeOfDayHours += TimeSpan.FromMinutes(minutes);

            var dt = DateTime.Today + timeOfDayHours;

            return dt;
        }
    }
}
