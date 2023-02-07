using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using trackingAPI.BackgroundHelpers;
using trackingAPI.Data;
using trackingAPI.Helpers;

namespace trackingAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //var serviceProvider = builder.Services.BuildServiceProvider();

        //using (var scope = serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
        //{
        //    var context = scope.ServiceProvider.GetService<DatabaseContext>();
        //}

        //builder.Services.AddHostedService<ImplementIHostedService>();
        builder.Services.AddHostedService<ImplementBackgroundService>();

        // Add services to the container.

        //builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //For anuglar app to consume the api (also with app.UseCors())
        builder.Services.AddCors();

        //setting sqlserver connection in appssetting.json
        builder.Services.AddDbContext<DatabaseContext>(
            o => o.UseSqlServer(
                builder.Configuration.
            GetConnectionString("SqlServer")));

        //Ensures that many to many models does not loop into each other lists
        builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


        var app = builder.Build();
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        ///Data Source="192.168.21.7, 1433";Initial Catalog=PatrickDB;User ID=Admin;Password=*********** 

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}