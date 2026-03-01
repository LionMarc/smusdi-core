using System.Net;
using System.Text.Json.Nodes;
using AwesomeAssertions;
using Reqnroll;

namespace Smusdi.Testing.Http;

[Binding]
public sealed class HttpSteps(ApiTesting apiTesting)
{
    /// <summary>
    /// Gets the last HTTP response returned by the <see cref="ApiTesting"/> helper.
    /// </summary>
    public HttpResponseMessage? ResponseMessage => apiTesting.ResponseMessage;

    /// <summary>
    /// When step: executes a GET request against the given URL.
    /// </summary>
    /// <param name="url">The request URL.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When(@"I execute the GET request {string}")]
    public Task WhenIExecuteTheGetRequest(string url) => apiTesting.Get(url);

    /// <summary>
    /// When step: executes a DELETE request against the given URL.
    /// </summary>
    /// <param name="url">The request URL.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When(@"I execute the DELETE request {string}")]
    public Task WhenIExecuteTheDeleteRequest(string url) => apiTesting.Delete(url);

    /// <summary>
    /// When step: executes a POST request with the given content.
    /// </summary>
    /// <param name="url">The request URL.</param>
    /// <param name="content">The request content as a string.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [When(@"I execute the POST request {string} with content")]
    public Task WhenIExecuteThePOSTRequestWithContent(string url, string content) => apiTesting.PostString(url, content);

    /// <summary>
    /// Then step: asserts that the received response has the expected HTTP status.
    /// </summary>
    /// <param name="expectedStatus">The expected HTTP status name (e.g. "OK").</param>
    [Then(@"I receive a {string} status")]
    public void ThenIReceiveAStatus(string expectedStatus)
    {
        this.ResponseMessage.Should().NotBeNull();
        this.ResponseMessage?.StatusCode.Should().Be(Enum.Parse<HttpStatusCode>(expectedStatus));
    }

    /// <summary>
    /// Then step: asserts that the response contains the validation errors described by the provided JSON.
    /// </summary>
    /// <param name="multilineText">Expected validation errors as JSON.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [Then(@"I receive the validation errors")]
    public async Task ThenIReceiveTheValidationErrors(string multilineText)
    {
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var receivedErrors = received?["errors"] ?? throw new InvalidOperationException("No errors received.");
        receivedErrors.ShouldBeSameJsonAs(multilineText);
    }

    /// <summary>
    /// Then step: asserts that the response problem detail matches the expected text.
    /// </summary>
    /// <param name="multilineText">Expected problem detail text.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [Then(@"I receive the problem detail")]
    public async Task ThenIReceiveTheProblemDetails(string multilineText)
    {
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        var received = JsonNode.Parse(receivedContent);

        var receivedDetail = received?["detail"]?.ToString();

        receivedDetail.Should().Be(multilineText);
    }

    /// <summary>
    /// Then step: asserts that the response body matches the provided JSON.
    /// </summary>
    /// <param name="multilineText">Expected response JSON.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [Then(@"I receive the response")]
    public async Task ThenIReceiveTheResponse(string multilineText)
    {
        var receivedContent = await (this.ResponseMessage?.Content.ReadAsStringAsync() ?? Task.FromResult("{}"));
        receivedContent.ShouldBeSameJsonAs(multilineText);
    }
}
