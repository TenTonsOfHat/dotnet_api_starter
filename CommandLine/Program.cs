// See https://aka.ms/new-console-template for more information

using Cocona;
using Microsoft.Extensions.Logging;
using Services.Refitter;
using Services.Weather;
using SharedEntrypoint;
using SharedEntrypoint.Logging;

var builder = CoconaApp.CreateBuilder(args);
builder.SetLogging();
builder.Services.RegisterAll();

var app = builder.Build();

app.AddCommand("weather", (IWeatherForecastService service, ILogger<Program> log) =>
{
    var weather = service.GetWeatherForecast();
    log.LogInformation("The weather is {Summary}!", weather.First().Summary);
});

app.AddCommand("pets", async (IPetApi service, ILogger<Program> log) =>
{
    var pets = await service.FindPetsByTags(new[] { "tag1" });
    log.LogInformation("The pet is {Summary}!", pets.Content?.First().Name);
});

await app.RunAsync();