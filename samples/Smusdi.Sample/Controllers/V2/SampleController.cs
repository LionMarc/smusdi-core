using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Sample.Controllers.V2;

[ApiController]
[ApiVersion("2")]
[Route("v{version:apiVersion}/sample")]
[ApiExplorerSettings(GroupName = "v2")]
public class SampleController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return this.Ok("Called from 2.0");
    }
}
