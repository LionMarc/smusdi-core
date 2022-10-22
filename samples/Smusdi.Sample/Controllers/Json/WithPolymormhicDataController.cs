using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Sample.Controllers.Json;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/polymorphic")]
[ApiExplorerSettings(GroupName = "v1")]
public class WithPolymormhicDataController : ControllerBase
{
    private static readonly List<Command> commands = new List<Command>();

    [HttpPost]
    public ActionResult<Command> Create([FromBody] Command command)
    {
        commands.Add(command);
        return this.Ok(command);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Command>> GetAll()
    {
        return this.Ok(commands);
    }
}
