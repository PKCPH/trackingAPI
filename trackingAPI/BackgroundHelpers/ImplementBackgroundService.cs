using trackingAPI.Configurations;

namespace trackingAPI.BackgroundHelpers;

public class ImplementBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;

    public ImplementBackgroundService(IServiceProvider services)
    {
        _services = services;
    }

    //Task running when IHostedSErvice starts
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (BackgroundTaskConfiguration.IsEnabled)
        {
            MatchBackgroundTask matchBackgroundTask = new();
            await matchBackgroundTask.CreateNewMatchesFromAvailableTeams(_services);
        }
        
        //maybe use PeriodicTimer or Timer for schedules match to be played
    }
}