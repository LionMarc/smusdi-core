namespace Smusdi.Core.Specs.Json;

public abstract class Stage
{
    protected Stage(StageType type, string name)
    {
        this.Type = type;
        this.Name = name;
    }

    public string Name { get; }

    public StageType Type { get; }
}
