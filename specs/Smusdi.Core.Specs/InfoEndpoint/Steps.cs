using Smusdi.Testing.Http;

namespace Smusdi.Core.Specs.InfoEndpoint;

[Binding]
public sealed class Steps
{
    private readonly ApiTesting apiTesting;

    public Steps(ApiTesting apiTesting) => this.apiTesting = apiTesting;

    [When(@"I request the info endpoint")]
    public Task WhenIRequestTheInfoEndpoint() => this.apiTesting.Get("info");
}
