using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Smusdi.Core.Validation;

namespace Smusdi.Core.Specs.Validation;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/projects")]
[ApiExplorerSettings(GroupName = "v1")]
public class ProjectsController(IValidator<Project> projectValidator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Project>> Create([FromBody] Project project)
    {
        var validationResult = await projectValidator.ValidateAsync(project);
        if (!validationResult.IsValid)
        {
            validationResult.CopyToModelState(this.ModelState);
            return this.ValidationProblem();
        }

        return this.Ok(project);
    }
}
