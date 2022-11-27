namespace Smusdi.Core.Specs.Json;

public abstract class Stage
{
    protected Stage(StageType type) => this.Type = type;

    public StageType Type { get; }
}
