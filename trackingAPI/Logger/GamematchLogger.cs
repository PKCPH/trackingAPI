using trackingAPI.Models;

namespace trackingAPI.Logger
{
    public class GamematchLogger
    {
        public static Task TimeLogger(Gamematch gameMatch, TimeSpan timeStamp, string _event)
        {
            TimeLog timeLog = new TimeLog
            {
                StartDateTime = DateTime.Now,
                TimeStamp = timeStamp,
                Event = _event,
                Gamematch = gameMatch
            };

            gameMatch.TimeLog.Add(timeLog);

            return Task.CompletedTask;
        }

       /* public int newTimeStamp(TimeLog timeLog)
        {

            var timespan = (timeLog.Gamematch.DateOfMatch - DateTime.Now).Ticks;

            timeLog.TimeStamp += timespan;

            return timespan;
        }*/
    }
}
