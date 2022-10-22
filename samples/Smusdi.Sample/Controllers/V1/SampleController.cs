using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Sample.Controllers.V1;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/sample")]
[ApiExplorerSettings(GroupName = "v1")]
public class SampleController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return this.Ok("Called");
    }
}
