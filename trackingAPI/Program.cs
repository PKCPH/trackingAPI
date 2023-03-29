using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using trackingAPI.BackgroundHelpers;
using trackingAPI.Data;
using trackingAPI.Helpers;
using Microsoft.AspNetCore.ResponseCompression;
using trackingAPI.Hubs;


namespace trackingAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<ImplementBackgroundService>();

        //SignalR (this is making sure that the webserver can process octet stream and adding compressions)
        //builder.Services.AddResponseCompression(opts =>
        //{
        //    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        //        new[] { "application/octet-stream" });
        //});
        builder.Services.AddSignalR(opt =>
        {
            opt.EnableDetailedErrors = true;
        });


        // Add services to the container.

        builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:5001",
                    ValidAudience = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                };
            });


        //builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //JWT Token service
        builder.Services.AddTransient<ITokenService, TokenService>();

        //For anuglar app to consume the api (also with app.UseCors())
        builder.Services.AddCors();

        //setting sqlserver connection in appssetting.json
        builder.Services.AddDbContext<DatabaseContext>(
            o => o.UseSqlServer(
                builder.Configuration.
            GetConnectionString("SqlServer")));

        //Ensures that many to many models does not loop into each other lists.
        builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        builder.Services.AddScoped<IPlayerService, PlayerService>();


        var app = builder.Build();
        //allowing the angular app to talk to the API
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //for UseEndpoints()
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
            endpoint.MapHub<TestHub>("/schedule");
        });

        app.MapControllers();

        app.Run();
    }
}