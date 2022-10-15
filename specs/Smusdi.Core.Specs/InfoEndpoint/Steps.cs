using Smusdi.Testing;

namespace Smusdi.Core.Specs.InfoEndpoint;

[Binding]
public sealed class Steps
{
    private readonly HttpSteps httpSteps;

    public Steps(HttpSteps httpSteps) => this.httpSteps = httpSteps;

    [When(@"I request the info endpoint")]
    public Task WhenIRequestTheInfoEndpoint() => this.httpSteps.WhenIExecuteTheGetRequest("info");
}
