using System.Net;
using System.Text.Json.Nodes;
using AwesomeAssertions;
using Reqnroll;

namespace Smusdi.Testing.Http;

[Binding]
public sealed class HttpSteps(ApiTesting apiTesting)
{
    public HttpResponseMessage? ResponseMessage => apiTesting.ResponseMessage;

    [When(@"I execute the GET request ""(.*)""")]
    public Task WhenIExecuteTheGetRequest(string url) => apiTesting.Get(url);

    [When(@"I execute the DELETE request ""(.*)""")]
    public Task WhenIExecuteTheDeleteRequest(string url) => apiTesting.Delete(url);

    [When(@"I execute the POST request ""(.*)"" with content")]
    public Task WhenIExecuteThePOSTRequestWithContent(string url, string content) => apiTesting.PostString(url, content);

    [Then(@"I receive a ""(.*)"" status")]
    public void ThenIReceiveAStatus(string expectedStatus)
    {
        this.ResponseMessage.Should().NotBeNull();
        this.ResponseMessage?.StatusCode.Should().Be(Enum.Parse<HttpStatusCode>(expectedStatus));
    }

    [Then(@"I receive the validation errors")]
    public async Task ThenIReceiveTheValidationErrors(string multilineText)
    {
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var receivedErrors = received?["errors"] ?? throw new InvalidOperationException("No errors received.");
        receivedErrors.ShouldBeSameJsonAs(multilineText);
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
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        receivedContent.ShouldBeSameJsonAs(multilineText);
    }
}
