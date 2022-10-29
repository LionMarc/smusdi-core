using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Core.Specs.Validation;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/projects")]
[ApiExplorerSettings(GroupName = "v1")]
public class ProjectsController : ControllerBase
{
    [HttpPost]
    public ActionResult<Project> Create([FromBody] Project project)
    {
        return this.Ok(project);
    }
}
