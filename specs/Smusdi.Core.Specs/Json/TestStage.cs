namespace Smusdi.Core.Specs.Json;

public class TestStage : Stage
{
    public TestStage(string name, int order)
        : base(StageType.Test, name)
    {
        this.Order = order;
    }

    public int Order { get; }
}
