namespace Smusdi.Core.Specs.Json;

public class BuildStage : Stage
{
    private readonly List<Stage> stages = new();

    public BuildStage(string name, ICollection<Stage>? stages = null)

        : base(StageType.Build, name)
    {
        if (stages != null)
        {
            this.stages.AddRange(stages);
        }
    }

    public ICollection<Stage> Stages => this.stages;
}
