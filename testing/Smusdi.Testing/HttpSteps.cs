using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.JsonDiffPatch;
using System.Text.Json.Nodes;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Smusdi.Testing;

[Binding]
public sealed class HttpSteps
{
    private readonly SmusdiTestingService smusdiTestingService;
    private HttpResponseMessage? responseMessage;

    public HttpSteps(SmusdiTestingService smusdiTestingService)
    {
        this.smusdiTestingService = smusdiTestingService;
    }

    [Given(@"identity with claims")]
    public void GivenIdentityWithClaims(Table table)
    {
        var claimsProvider = this.smusdiTestingService.GetService<ClaimsProvider>();
        if (claimsProvider == null)
        {
            return;
        }

        foreach (var row in table.Rows)
        {
            claimsProvider.AddClaim(row["Type"], row["Value"]);
        }
    }

    [When(@"I execute the POST request ""(.*)"" with content")]
    public async Task WhenIExecuteThePOSTRequestWithContent(string url, string content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json) };
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        this.responseMessage = await this.smusdiTestingService.TestClient.SendAsync(request);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    [Then(@"I should receive a ""(.*)"" status")]
    public void ThenIShouldReceiveAStatus(string expectedStatus)
    {
        this.responseMessage.Should().NotBeNull();
        this.responseMessage?.StatusCode.Should().Be(Enum.Parse<HttpStatusCode>(expectedStatus));
    }

    [Then(@"I should receive the validation errors")]
    public async Task ThenIShouldReceiveTheValidationErrors(string multilineText)
    {
        var expected = JsonNode.Parse(multilineText);
        var receivedContent = await (this.responseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var receivedErrors = received?["errors"];

        var diff = receivedErrors.Diff(expected);

        diff.Should().BeNull();
    }

    [Then(@"I should receive the response")]
    public async Task ThenIShouldReceiveTheResponse(string multilineText)
    {
        var expected = JsonNode.Parse(multilineText);
        var receivedContent = await (this.responseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var diff = received.Diff(expected);

        diff.Should().BeNull();
    }
}
