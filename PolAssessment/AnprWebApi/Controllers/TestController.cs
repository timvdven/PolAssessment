using Microsoft.AspNetCore.Mvc;

namespace PolAssessment.AnprWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet(Name = "Test")]
    public IEnumerable<string> Get()
    {
        return ["This", "Is", "A", "Test"];
    }
}
