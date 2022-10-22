namespace Smusdi.Sample.Controllers.Json;

public abstract class Command
{
    protected Command(string type) => this.Type = type;

    public string Type { get; }
}
