using System.Text.Json;
using AwesomeAssertions;
using Smusdi.Testing.Http;

namespace Smusdi.Core.Specs.OpenApi;

[Binding]
public sealed class Steps(ApiTesting apiTesting)
{
    [Then("the response is an OpenAPI document")]
    public async Task ThenTheResponseIsAnOpenApiDocument()
    {
        var responseContent = await apiTesting.GetResponseMessageOrThrow().Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseContent);

        document.RootElement.TryGetProperty("openapi", out var openApiVersion).Should().BeTrue();
        openApiVersion.GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Then("the OpenAPI server is {string}")]
    public async Task ThenTheOpenApiServerIs(string expectedServer)
    {
        var responseContent = await apiTesting.GetResponseMessageOrThrow().Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseContent);

        document.RootElement.GetProperty("servers")[0].GetProperty("url").GetString().Should().Be(expectedServer);
    }
}
