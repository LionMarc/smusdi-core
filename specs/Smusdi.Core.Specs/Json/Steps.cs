using System.IO.Abstractions;
using Smusdi.Core.Json;
using Smusdi.Testing;

namespace Smusdi.Core.Specs.Json;

[Binding]
public class Steps
{
    private readonly IJsonSerializer jsonSerializer;
    private readonly IFileSystem fileSystem;
    private Workflow? workflow;
    private List<Workflow>? workflows;

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
        var expected = new StandardWorklflow(new List<Stage> { new BuildStage(), new TestStage() });
        this.workflow.Should().BeEquivalentTo(expected);
    }

    [When(@"I serialize a standard workflow with a build stage to the file ""(.*)""")]
    public async Task WhenISerializeAStandardWorkflowWithABuildStageToTheFile(string filePath)
    {
        this.fileSystem.Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        this.workflow = new StandardWorklflow(new List<Stage> { new BuildStage() });
        using var stream = this.fileSystem.File.OpenWrite(filePath);
        await this.jsonSerializer.SerializeAsync(this.workflow, stream);
    }
}
