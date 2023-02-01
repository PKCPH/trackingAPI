using Microsoft.AspNetCore.Mvc;

namespace trackingAPI.Helpers
{
    public class MatchBackgroundTasks : BackgroundService
    {

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("From MybackgroundService: Execute Async");
            return Task.CompletedTask;

        }
    }
}
