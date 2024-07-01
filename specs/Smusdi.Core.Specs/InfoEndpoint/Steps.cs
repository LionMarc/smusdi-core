using Microsoft.Extensions.Options;
using Smusdi.Testing;
using Smusdi.Testing.Http;

namespace Smusdi.Core.Specs.InfoEndpoint;

[Binding]
public sealed class Steps(ApiTesting apiTesting, SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    private readonly ApiTesting apiTesting = apiTesting;
    private readonly IOptions<SmusdiOptions> smusdiOptions = smusdiServiceTestingSteps.SmusdiTestingService.GetRequiredService<IOptions<SmusdiOptions>>();

    [Given("the info cache disabled")]
    public void GivenTheInfoCacheDisabled() => this.smusdiOptions.Value.InfoCacheDisabled = true;

    [Given("the info cache enabled")]
    public void GivenTheInfoCacheEnabled() => this.smusdiOptions.Value.InfoCacheDisabled = false;

    [When(@"I request the info endpoint")]
    [Given("the info endpoint requested")]
    public Task WhenIRequestTheInfoEndpoint() => this.apiTesting.Get("info");
}
