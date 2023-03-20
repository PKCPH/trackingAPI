using System.Net;
using Newtonsoft.Json;
using trackingAPI.Configurations;
using static trackingAPI.Configurations.BackgroundTaskConfiguration;

namespace trackingAPI.Helpers;

public static class DateTimePicker
{
    
    public static DateTime CreateRandomMatchTime()
    {

        var rnd = new Random();
        DateTime dateTime = GetCopenhagenTimeFromWeb();
        var date = dateTime.Date; //.today when not testing

        //Timespan of the random scheduled time 
        var minutes = rnd.Next(0, ScheduledTimeSpanInMinutes);
        //when the match starts the earliest on a given day
        var timeOfDayHours = TimeSpan.FromHours(StartHourOfScheduledTimeSpan);
        timeOfDayHours += TimeSpan.FromMinutes(minutes);
        var pickedDateTime = date + timeOfDayHours;

        //if pickedDateTime is before datetime return it with +1 day else return its normal datetime
        return (pickedDateTime < dateTime) ? pickedDateTime.AddDays(1) : pickedDateTime;
    }

    //Api call for Copenhagen Time
    //can be more dynamic for ip adress
    public static DateTime GetCopenhagenTimeFromWeb()
    {
        var req = WebRequest.CreateHttp("http://worldtimeapi.org/api/timezone/Europe/Copenhagen");
        req.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => certificate.GetCertHashString() == "<real_Hash_here>";
        var response = req.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var responseText = streamReader.ReadToEnd();
            WorldTime worldTime = JsonConvert.DeserializeObject<WorldTime>(responseText);
            Console.WriteLine(worldTime.DateTime);
            return worldTime.DateTime;
        }

    }
}

class WorldTime
{
    public DateTime DateTime { get; set; }
}
