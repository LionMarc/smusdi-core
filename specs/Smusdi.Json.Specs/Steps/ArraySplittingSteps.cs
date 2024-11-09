using System.IO;
using System.Text;
using FluentAssertions;
using Reqnroll;

namespace Smusdi.Json.Specs.Steps;

[Binding]
public sealed class ArraySplittingSteps
{
    private readonly List<string> jsonArrayItems = [];
    private Exception? caughtException = null;

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

    private sealed record ArrayItem(string Value);
}
