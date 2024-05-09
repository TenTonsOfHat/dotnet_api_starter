using System.Text.Json;
using Microsoft.Extensions.Logging;
using Services.Refitter;

namespace Tests;

public class ExampleTest(ILogger<ExampleTest> logger, IPetApi api)
{
    [Fact]
    public async Task TestFindPetsByStatus()
    {
        var pets = await api.FindPetsByStatus(new Anonymous[] { Anonymous.Available });
        logger.LogInformation("The pet is {Summary}!", pets.Content?.First().Name);
    }
}