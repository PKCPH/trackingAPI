using System.Diagnostics;
using System.Timers;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;


public class LiveMatchBackgroundTask
{
    //private readonly PeriodicTimer _timer = new(LiveMatchConfiguration.TimerTickTimeSpan);
    //private System.Timers.Timer _timer = new System.Timers.Timer();
    private readonly IServiceProvider _services;
    public LiveMatchBackgroundTask(IServiceProvider services)
    {
        _services = services;
    }

    public async Task ExecuteLiveMatch(PeriodicTimer timer)
    {
        //_timer.Interval = 1000;
        
        int count = 0;

        using (var scope = _services.CreateScope())
        {
            var _context =
                scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();
            while (await timer.WaitForNextTickAsync())
            {
                //Console.WriteLine($"{Convert.ToInt32(_timer.ToString().PadLeft(2, '0'))} ");
                //$": {Convert.ToInt32(stopwatch.Elapsed.TotalSeconds).ToString().PadLeft(2, '0')}");
                Console.WriteLine(count);

                count++;

            }
        }
    }
}
