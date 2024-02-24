using System.Net.Mime;
using System.Text;
using System.Text.Json.JsonDiffPatch;
using System.Text.Json.Nodes;
using FluentAssertions;
using Smusdi.Core.Json;

namespace Smusdi.Testing.Http;

public static class JsonNodeExtensions
{
    public static void ShoildBeSameAs(this JsonNode jsonNode, string expected)
    {
        var expectedAsJsonNode = JsonNode.Parse(expected);
        var diff = jsonNode.Diff(expectedAsJsonNode);
        diff.Should().BeNull();
    }
}
