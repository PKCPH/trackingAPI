using trackingAPI.BackgroundHelpers;

namespace WebApplication3.Services;

public class ImplementBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;

    public ImplementBackgroundService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        MatchBackgroundTask matchBackgroundTask = new();
        matchBackgroundTask.CreateNewMatchesFromAvailableTeams(_services);
        
    }
}