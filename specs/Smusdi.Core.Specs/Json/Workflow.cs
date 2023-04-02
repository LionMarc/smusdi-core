namespace Smusdi.Core.Specs.Json;

public class Workflow
{
    public Workflow(WorkflowType type, ICollection<Stage> stages)
    {
        this.Type = type;
        this.Stages = stages;
    }

    public WorkflowType Type { get; }

    public ICollection<Stage> Stages { get; }
}
