namespace Smusdi.Core.Specs.Json;

public class StandardWorklflow : Workflow
{
    public StandardWorklflow(ICollection<Stage> stages)
        : base(WorkflowType.Standard, stages)
    {
    }
}
