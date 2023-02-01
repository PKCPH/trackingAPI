using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;

namespace trackingAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
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