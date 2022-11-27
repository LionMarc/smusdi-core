namespace Smusdi.Core.Specs.Json;

public class SimplifiedWorklflow : Workflow
{
    public SimplifiedWorklflow(ICollection<Stage> stages)
        : base(WorkflowType.Simplified, stages)
    {
    }
}
