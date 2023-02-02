using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;

namespace WebApplication3.Services;
//https://social.msdn.microsoft.com/Forums/en-US/bc640f9d-2293-4570-86f2-71b25e6ac95b/inject-dbcontext-in-ihostedservice
public class ImplementIHostedService
{
    private DatabaseContext _context;
    public ImplementIHostedService(IServiceProvider serviceProvider)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            _context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Response from IHostedService - {DateTime.Now}");
                CreateRandomMatch();
                await Task.Delay(10000);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task CreateRandomMatch()
    {
        MatchController matchController = new(_context);
        TeamPicker teamPicker = new TeamPicker();
        await matchController.Create(teamPicker);
        Console.WriteLine("create random match done");
    }

}

