using Microsoft.AspNetCore.Mvc;

namespace Smusdi.ApiWithNoVersioning.Controllers;

[ApiController]
[Route("records")]
public class RecordsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return this.Ok();
    }
}
