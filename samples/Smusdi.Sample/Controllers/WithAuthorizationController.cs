using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Sample.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/with-authorization")]
public class WithAuthorizationController : ControllerBase
{
    [HttpGet("with-no-scope")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public ActionResult WithNoScope()
    {
        return this.Ok("No scope");
    }

    [HttpGet("requires-scope1")]
    [Authorize("scope1")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public ActionResult RequiresScope1()
    {
        return this.Ok("Scope1");
    }

    [HttpGet("requires-scope2")]
    [Authorize("scope2")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public ActionResult RequiresScope2()
    {
        return this.Ok("Scope2");
    }
}
