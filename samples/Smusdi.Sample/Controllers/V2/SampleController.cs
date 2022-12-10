using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Smusdi.Core.Helpers;

namespace Smusdi.Sample.Controllers.V2;

[ApiController]
[ApiVersion(2.0)]
[Route("v{version:apiVersion}/sample")]
public class SampleController : ControllerBase
{
    private readonly IClock clock;

    public SampleController(IClock clock) => this.clock = clock;

    [HttpGet]
    public IActionResult Get()
    {
        return this.Ok($"Called from 2.0 at {this.clock.UtcNow}");
    }
}
