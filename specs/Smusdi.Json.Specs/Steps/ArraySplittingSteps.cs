using System.Text;
using System.Text.Json;
using AwesomeAssertions;
using Reqnroll;
using Smusdi.Testing;

namespace Smusdi.Json.Specs.Steps;

[Binding]
public sealed class ArraySplittingSteps
{
    private readonly List<string> jsonArrayItems = [];
    private Exception? caughtException = null;
    private string? utf8JsonWriterContent = null;

    [When("I split the json array")]
    public void WhenISplitTheJsonArray(string multilineText)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(multilineText));
        this.SplitStream(memoryStream);
    }

    [When("I split json array from file {string}")]
    public void WhenISplitJsonArrayFromFile(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        this.SplitStream(stream);
    }

    [When("I split the json array into a Utf8JsonWriter")]
    public void WhenISplitTheJsonArrayIntoAUtfJsonWriter(string multilineText)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(multilineText));
        this.SplitStreamIntoUtf8JsonWriter(memoryStream);
    }

    [Then("a {string} exception is thrown")]
    public void ThenAExceptionIsThrown(string exceptionType, string exceptionMessage)
    {
        this.caughtException.Should().NotBeNull();
        this.caughtException?.GetType().Name.Should().Be(exceptionType);
        this.caughtException?.Message.Should().Be(exceptionMessage);
    }

    [Then("the array items are found")]
    public void ThenTheArrayItemsAreFound(DataTable dataTable)
    {
        var expected = dataTable.CreateSet<ArrayItem>().Select(i => i.Value).ToList();
        this.jsonArrayItems.Should().BeEquivalentTo(expected);
    }

    [Then("the output json is")]
    public void ThenTheOutputJsonIs(string multilineText)
    {
        (this.utf8JsonWriterContent ?? string.Empty).ShouldBeSameJsonAs(multilineText);
    }

    private void SplitStream(Stream stream)
    {
        try
        {
            var splitter = new JsonArraySplitter(stream);
            while (true)
            {
                using var outputStream = new MemoryStream();
                var hasItem = splitter.ReadNextItem(outputStream);
                if (!hasItem)
                {
                    return;
                }

                outputStream.Seek(0, SeekOrigin.Begin);
                var data = outputStream.ToArray();
                this.jsonArrayItems.Add(Encoding.UTF8.GetString(data));
            }
        }
        catch (Exception e)
        {
            this.caughtException = e;
        }
    }

    private void SplitStreamIntoUtf8JsonWriter(Stream stream)
    {
        try
        {
            var splitter = new JsonArraySplitter(stream);
            using var outputStream = new MemoryStream();
            using var utf8JsonWriter = new Utf8JsonWriter(outputStream);
            utf8JsonWriter.WriteStartArray();
            while (true)
            {
                var hasItem = splitter.ReadNextItem(utf8JsonWriter);
                if (!hasItem)
                {
                    break;
                }
            }

            utf8JsonWriter.WriteEndArray();
            utf8JsonWriter.Flush();
            outputStream.Seek(0, SeekOrigin.Begin);
            var data = outputStream.ToArray();
            this.utf8JsonWriterContent = Encoding.UTF8.GetString(data);
        }
        catch (Exception e)
        {
            this.caughtException = e;
        }
    }

    private sealed record ArrayItem(string Value);
}
