using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Sample.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/sample")]
public class SampleController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return this.Ok("Called");
    }
}
