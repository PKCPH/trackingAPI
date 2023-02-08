using trackingAPI.Configurations;
using trackingAPI.Models;

namespace trackingAPI.BackgroundHelpers;

public class ImplementBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    //parameter is how often the Timer ticks (set to once every second)
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(1000));

    public ImplementBackgroundService(IServiceProvider services)
    {
        _services = services;
    }

    //Task running when IHostedSErvice starts
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (await _timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested)
        {
            MatchBackgroundTask matchBackgroundTask = new(_services);
            await matchBackgroundTask.CreateNewMatchesFromAvailableTeams();
            await matchBackgroundTask.FindAndPlayMatches(stoppingToken, _timer);
            
            //await Task.Delay(5000);
        }
        //maybe use PeriodicTimer or Timer for schedules match to be played
    }
}