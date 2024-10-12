using System.Text;
using FluentAssertions;
using Reqnroll;

namespace Smusdi.Json.Specs.Steps;

[Binding]
public sealed class ArraySplittingSteps
{
    private List<string> jsonArrayItems = [];
    private Exception? caughtException = null;

    [When("I split the json array")]
    public void WhenISplitTheJsonArray(string multilineText)
    {
        try
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(multilineText));
            var splitter = new JsonArraySplitter(memoryStream);
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

    private sealed record ArrayItem(string Value);
}
