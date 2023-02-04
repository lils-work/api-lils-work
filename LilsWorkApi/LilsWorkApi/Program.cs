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

            #region Task

            var connectionString = builder.Configuration["ConnectionStrings:taskConnection"];
            builder.Services.AddDbContext<TaskDbContext>(optionsBuilder =>
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddDataProtection();

            #endregion

            var app = builder.Build();
            app.Logger.LogInformation(connectionString);

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

            app.Run();
        }
    }
}