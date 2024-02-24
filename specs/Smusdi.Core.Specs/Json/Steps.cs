using System.IO.Abstractions;
using System.Text.Json.JsonDiffPatch;
using System.Text.Json.Nodes;
using Smusdi.Core.Json;
using Smusdi.Testing;

namespace Smusdi.Core.Specs.Json;

[Binding]
public class Steps
{
    private readonly IJsonSerializer jsonSerializer;
    private readonly IFileSystem fileSystem;
    private readonly Dictionary<string, Workflow> workflowByName = new();
    private Workflow? workflow;
    private List<Workflow>? workflows;
    private string? serializedWorkflow;

    public Steps(SmusdiTestingService smusdiTestingService)
    {
        this.jsonSerializer = smusdiTestingService.GetService<IJsonSerializer>()!;
        this.fileSystem = smusdiTestingService.GetService<IFileSystem>()!;
    }

    [When(@"I deserialize the workflow")]
    public void WhenIDeserializeTheWorkflow(string multilineText)
    {
        this.workflow = this.jsonSerializer.Deserialize<Workflow>(multilineText);
    }

    [When(@"I deserialize the workflow list")]
    public void WhenIDeserializeTheWorkflowList(string multilineText)
    {
        this.workflows = this.jsonSerializer.Deserialize<List<Workflow>>(multilineText);
    }

    [Then(@"I get a valid workflow")]
    public void ThenIGetAValidWorkflow()
    {
        this.workflow.Should().NotBeNull();
        var expected = new StandardWorklflow(new Stage[]
        {
            new BuildStage("first", new[] { new TestStage("Third", 1) }),
            new TestStage("second", 2),
        });

        var actual = JsonNode.Parse(this.jsonSerializer.Serialize(this.workflow));
        var expectedNode = JsonNode.Parse(this.jsonSerializer.Serialize(expected));

        actual.Diff(expectedNode).Should().BeNull();
    }

    [Then(@"I get a valid list of workflow")]
    public void ThenIGetAValidListOfWorkflow()
    {
        this.workflows.Should().NotBeNull();
    }

    [When(@"I deserialize the workflow from the file ""(.*)""")]
    public async Task WhenIDeserializeTheWorkflowFromTheFile(string path)
    {
        using var stream = this.fileSystem.File.OpenRead(path);
        this.workflow = await this.jsonSerializer.DeserializeAsync<Workflow>(stream);
    }

    [Then(@"I get the right workflow")]
    public void ThenIGetTheRightWorkflow()
    {
        var expected = new StandardWorklflow(new List<Stage> { new BuildStage("first"), new TestStage("second", 1) });
        this.workflow.Should().BeEquivalentTo(expected);
    }

    [When(@"I serialize a standard workflow with a build stage to the file ""(.*)""")]
    public async Task WhenISerializeAStandardWorkflowWithABuildStageToTheFile(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            this.fileSystem.Directory.CreateDirectory(directory);
        }

        this.workflow = new StandardWorklflow(new List<Stage> { new BuildStage("first") });
        using var stream = this.fileSystem.File.OpenWrite(filePath);
        await this.jsonSerializer.SerializeAsync(this.workflow, stream);
    }

    [Given(@"the deserialized workflow ""(.*)""")]
    public void GivenTheDeserializedWorkflow(string name, string multilineText)
    {
        var expectedWorkflow = this.jsonSerializer.Deserialize<Workflow>(multilineText) ?? throw new InvalidOperationException("unable to deserialize input text to Wrokflow.");
        this.workflowByName[name] = expectedWorkflow;
    }

    [When(@"I serialized the workflow ""(.*)""")]
    public void WhenISerializedTheWorkflow(string name)
    {
        this.serializedWorkflow = this.jsonSerializer.Serialize(this.workflowByName[name]);
    }

    [Then(@"I get the result")]
    public void ThenIGetTheResult(string multilineText)
    {
        var expected = JsonNode.Parse(multilineText);
        var serialized = JsonNode.Parse(this.serializedWorkflow!);
        var diff = serialized.Diff(expected);

        diff.Should().BeNull();
    }
}
