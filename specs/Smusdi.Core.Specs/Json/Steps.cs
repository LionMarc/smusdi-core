using Smusdi.Core.Json;
using Smusdi.Testing;

namespace Smusdi.Core.Specs.Json;

[Binding]
public class Steps
{
    private readonly IJsonSerializer jsonSerializer;
    private Workflow? workflow;
    private List<Workflow>? workflows;

    public Steps(SmusdiTestingService smusdiTestingService)
    {
        this.jsonSerializer = smusdiTestingService.GetService<IJsonSerializer>()!;
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
}
