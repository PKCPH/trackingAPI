using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace WebApplication3.Services;

public class ImplementIHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Respponse from IHostedService - {DateTime.Now}");
                await Task.Delay(1000);
            }
        });

        return Task.CompletedTask;
        //< span public Task StopAsync(CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //class="hljs-built_in">return</span> <span class="hljs-built_in">Task</span>.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    //<span class="hljs-keyword">public</span> <span class="hljs-built_in">Task</span> StopAsync(CancellationToken cancellationToken)
    //{
    //    < span class="hljs-built_in">Console</span>.WriteLine(<span class="hljs-string">"Shutting down IHostedService"</span>);
    //    <span class="hljs-built_in">return</span> <span class="hljs-built_in">Task</span>.CompletedTask;

    //}

}

