namespace Smusdi.Core.Specs.Validation;

public class Project
{
    public string Name { get; set; } = string.Empty;

    public Target Target { get; set; } = new Target();
}
