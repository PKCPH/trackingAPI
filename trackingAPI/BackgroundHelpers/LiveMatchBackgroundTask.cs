using System.Diagnostics;
using System.Timers;
using trackingAPI.Configurations;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;


public class LiveMatchBackgroundTask
{
    //private readonly PeriodicTimer _timer = new(LiveMatchConfiguration.TimerTickTimeSpan);
    private static System.Timers.Timer _timer = new System.Timers.Timer(1000);
    private readonly IServiceProvider _services;
    private readonly DatabaseContext _context;
    private IServiceProvider services;

    public LiveMatchBackgroundTask(IServiceProvider services)
    {
        this.services = services;
    }

    public Task ExecuteLiveMatch(GameMatch gameMatch)
    {
        int count = 0;

        _timer.Elapsed += _timer.Elapsed;

        //Console.WriteLine($"{timer.ToString().PadLeft(2, '0')} ");
        // $": {Convert.ToInt32(stopwatch.Elapsed.TotalSeconds).ToString().PadLeft(2, '0')}");
        while (count < 5400)
        {
            TimeSpan result = TimeSpan.FromSeconds(count);
            string fromTimeString = result.ToString("mm':'ss");
            Console.WriteLine(fromTimeString);
            count++;
        }


        return Task.CompletedTask;
        //timer.WaitForNextTickAsync(stoppingToken);               
    }



}

