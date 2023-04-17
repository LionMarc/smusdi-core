namespace Smusdi.Core.Specs.Json;

public class ListStage : Stage
{
    public ListStage(string name, IEnumerable<Stage> stages)
        : base(StageType.List, name)
    {
        this.Stages = stages;
    }

    public IEnumerable<Stage> Stages { get; }
}
