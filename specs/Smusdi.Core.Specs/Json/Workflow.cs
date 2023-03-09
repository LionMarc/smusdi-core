namespace Smusdi.Core.Specs.Json;

public abstract class Workflow
{
    protected Workflow(WorkflowType type, ICollection<Stage> stages)
    {
        this.Type = type;
        this.Stages = stages;
    }

    public WorkflowType Type { get; }

    public ICollection<Stage> Stages { get; }
}
