using System.Text.Json.JsonDiffPatch;
using System.Text.Json.Nodes;
using AwesomeAssertions;

namespace Smusdi.Testing;

public static class JsonTestingHelpers
{
    public static void ShouldBeSameJsonAs(this string received, string expected)
    {
        var receivedJson = JsonNode.Parse(received) ?? throw new InvalidOperationException("Cannot parse received as json.");
        receivedJson.ShouldBeSameJsonAs(expected);
    }

    public static void ShouldBeSameJsonAs(this JsonNode received, string expected)
    {
        var expectedJson = JsonNode.Parse(expected);
        var diff = received.Diff(expectedJson);
        diff.Should().BeNull(diff?.ToJsonString());
    }
}
