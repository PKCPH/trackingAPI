using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using trackingAPI.Controllers;
using trackingAPI.Data;
using trackingAPI.Helpers;

namespace WebApplication3.Services
{
    public class ImplementBackgroundService : BackgroundService
    {
        //private readonly DatabaseContext _context;

        //public ImplementBackgroundService(DatabaseContext context)
        //{
        //    _context = context;
        //}



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //MatchController matchController = new(_context);
            //TeamPicker teamPicker = new TeamPicker();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Response from Background Service - {DateTime.Now}");
                //await matchController.Create(teamPicker);
                await CreateMatchBackgroundTask();
                await Task.Delay(10000);
            }

        }

        private static Task CreateMatchBackgroundTask()
        {
            Console.WriteLine("123");
            // your code here
            return Task.FromResult("Task done");
        }
    }
}