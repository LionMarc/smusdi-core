using System.Text.Json.Nodes;
using AwesomeAssertions;
using Microsoft.Extensions.Options;
using Smusdi.Testing;
using Smusdi.Testing.Http;

namespace Smusdi.Core.Specs.InfoEndpoint;

[Binding]
public sealed class Steps(ApiTesting apiTesting, SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    private readonly ApiTesting apiTesting = apiTesting;
    private readonly IOptions<SmusdiOptions> smusdiOptions = smusdiServiceTestingSteps.GetRequiredService<IOptions<SmusdiOptions>>();

    [Given("the info cache disabled")]
    public void GivenTheInfoCacheDisabled() => this.smusdiOptions.Value.InfoCacheDisabled = true;

    [Given("the info cache enabled")]
    public void GivenTheInfoCacheEnabled() => this.smusdiOptions.Value.InfoCacheDisabled = false;

    [Then("I receive the info response")]
    public async Task ThenIReceiveTheInfoResponse(string multilineText)
    {
        var response = this.apiTesting.GetResponseMessageOrThrow();
        var receivedContent = await response.Content.ReadAsStringAsync();
        var received = JsonNode.Parse(receivedContent) ?? throw new InvalidOperationException("Cannot parse received as json.");

        var dotnetVersion = received["dotnetVersion"]?.GetValue<string>()
            ?? throw new InvalidOperationException("The response does not include dotnetVersion.");
        dotnetVersion.Should().Be(Environment.Version.ToString());
        received.AsObject().Remove("dotnetVersion");
        received.ShouldBeSameJsonAs(multilineText);
    }

    [When(@"I request the info endpoint")]
    [Given("the info endpoint requested")]
    public Task WhenIRequestTheInfoEndpoint() => this.apiTesting.Get("info");
}
