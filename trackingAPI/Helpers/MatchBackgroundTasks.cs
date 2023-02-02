using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers;

public class MatchBackgroundTasks : BackgroundService
{

    private readonly ILogger<MatchBackgroundTasks> _logger;
    /// <summary>
    /// accessing the database at runtime
    /// </summary>
    private int executionCount = 0;
    private Timer? _timer = null;

    //constructor for implementing logger in background task
    public MatchBackgroundTasks(ILogger<MatchBackgroundTasks> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _logger.LogInformation("From MybackgroundService: Execute Async");

        //await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            //_timer = new Timer(DoWork, null, TimeSpan.FromSeconds(5),
            //            TimeSpan.FromSeconds(15));
        }

        return base.StartAsync(cancellationToken);
    }

    //private void DoWork(object? state)
    //{
    //    var count = Interlocked.Increment(ref executionCount);

    //    _logger.LogInformation(
    //        "Timed Hosted Service is working. Count: {Count}", count);
    //}
}
