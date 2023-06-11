using System.Text.Json.Serialization;
using LilsWorkApi.Services;
using Microsoft.EntityFrameworkCore;

namespace LilsWorkApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddAuthorization();

            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("allowpaired", builder =>
                {
                    builder
                        .WithOrigins(
                            "http://lils.work",
                            "http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            #endregion

            #region Task

            var connectionString = builder.Configuration["ConnectionStrings:taskConnection"];
            if (File.Exists("./secretConnectionString.txt"))
                connectionString = File.ReadAllText("./secretConnectionString.txt");
            builder.Services.AddDbContext<TaskDbContext>(optionsBuilder =>
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddDataProtection();

            builder.Services.AddHostedService<TaskGenerationService>();

            #endregion

            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                forecast.Last().Summary = "Happy day!!!";
                return forecast;
            });

            app.MapControllers();
            app.UseCors();

            app.Run();
        }
    }
}