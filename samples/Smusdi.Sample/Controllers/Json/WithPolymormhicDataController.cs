using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Sample.Controllers.Json;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/polymorphic")]
[ApiExplorerSettings(GroupName = "Json")]
public class WithPolymormhicDataController : ControllerBase
{
    private static readonly List<Command> Commands = new List<Command>();

    [HttpPost]
    public ActionResult<Command> Create([FromBody] Command command)
    {
        Commands.Add(command);
        return this.Ok(command);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Command>> GetAll()
    {
        return this.Ok(Commands);
    }
}
