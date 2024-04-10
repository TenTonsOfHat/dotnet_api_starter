using Microsoft.Extensions.Logging;
using Services.Abstraction;

namespace Services.Weather;

public class WeatherForecastService(
    ILogger<WeatherForecastService> logger
) : IWeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        logger.LogInformation("Getting weather forecast");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}

public interface IWeatherForecastService : ITransientService
{
    IEnumerable<WeatherForecast> GetWeatherForecast();
}