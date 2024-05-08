using Microsoft.AspNetCore.Mvc;
using Services.Refitter;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PetController(
    ILogger<PetController> logger,
    IPetApi petApi
) : ControllerBase
{
    [HttpGet(Name = "FindPetsByTags")]
    public async Task<ICollection<Pet>?> FindPetsByTags()
    {
        var response = await petApi.FindPetsByTags(new[] { "tag1" });
        logger.LogInformation("StatusCode: {response}", response.StatusCode);
        return response.Content;
    }
}