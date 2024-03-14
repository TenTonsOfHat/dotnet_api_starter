using dotnet_api_starter.Services.Weather;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_api_starter.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(
    ILogger<WeatherForecastController> logger,
    IWeatherForecastService weatherForecastService
) : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return weatherForecastService.GetWeatherForecast();
    }
}