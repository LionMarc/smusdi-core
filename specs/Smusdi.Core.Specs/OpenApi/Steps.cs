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

    [Then("the OpenAPI info contains title {string}, description {string}, and contact email {string}")]
    public async Task ThenTheOpenApiInfoContains(string expectedTitle, string expectedDescription, string expectedContactEmail)
    {
        var responseContent = await apiTesting.GetResponseMessageOrThrow().Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseContent);

        var info = document.RootElement.GetProperty("info");
        info.GetProperty("title").GetString().Should().Be(expectedTitle);
        info.GetProperty("description").GetString().Should().Be(expectedDescription);
        info.GetProperty("contact").GetProperty("email").GetString().Should().Be(expectedContactEmail);
    }
}
