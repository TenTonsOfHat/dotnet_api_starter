using dotnet_api_starter.Services.Abstraction;

namespace dotnet_api_starter.Services.Weather;

public class WeatherForecastService(
    ILogger<WeatherForecastService> logger
) : IWeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastService> _logger = logger;

    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        _logger.LogInformation("Getting weather forecast");
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