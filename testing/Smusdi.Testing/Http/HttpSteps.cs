using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.JsonDiffPatch;
using System.Text.Json.Nodes;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Smusdi.Testing.Http;

[Binding]
public sealed class HttpSteps
{
    private readonly SmusdiTestingService smusdiTestingService;

    public HttpSteps(SmusdiServiceTestingSteps steps)
    {
        this.smusdiTestingService = steps.SmusdiTestingService;
    }

    public HttpResponseMessage? ResponseMessage { get; set; }

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
            var appendToExistingClaim = true;
            if (row.TryGetValue("append", out var append) && !string.IsNullOrWhiteSpace(append))
            {
                appendToExistingClaim = bool.Parse(append);
            }

            claimsProvider.AddClaim(row["Type"], row["Value"], appendToExistingClaim);
        }
    }

    [When(@"I execute the GET request ""(.*)""")]
    public async Task WhenIExecuteTheGetRequest(string url)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        this.ResponseMessage = await this.smusdiTestingService.TestClient.GetAsync(url);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    [When(@"I execute the DELETE request ""(.*)""")]
    public async Task WhenIExecuteTheDeleteRequest(string url)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        this.ResponseMessage = await this.smusdiTestingService.TestClient.DeleteAsync(url);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    [When(@"I execute the POST request ""(.*)"" with content")]
    public async Task WhenIExecuteThePOSTRequestWithContent(string url, string content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json) };
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        this.ResponseMessage = await this.smusdiTestingService.TestClient.SendAsync(request);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    [Then(@"I receive a ""(.*)"" status")]
    public void ThenIReceiveAStatus(string expectedStatus)
    {
        this.ResponseMessage.Should().NotBeNull();
        this.ResponseMessage?.StatusCode.Should().Be(Enum.Parse<HttpStatusCode>(expectedStatus));
    }

    [Then(@"I receive the validation errors")]
    public async Task ThenIReceiveTheValidationErrors(string multilineText)
    {
        var expected = JsonNode.Parse(multilineText);
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var receivedErrors = received?["errors"];

        var diff = receivedErrors.Diff(expected);

        diff.Should().BeNull();
    }

    [Then(@"I receive the problem detail")]
    public async Task ThenIReceiveTheProblemDetails(string multilineText)
    {
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var receivedDetail = received?["detail"]?.ToString();

        receivedDetail.Should().Be(multilineText);
    }

    [Then(@"I receive the response")]
    public async Task ThenIReceiveTheResponse(string multilineText)
    {
        var expected = JsonNode.Parse(multilineText);
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var diff = received.Diff(expected);

        diff.Should().BeNull();
    }

    public async Task Send(HttpRequestMessage request)
    {
        this.ResponseMessage = await this.smusdiTestingService.TestClient!.SendAsync(request);
    }
}
