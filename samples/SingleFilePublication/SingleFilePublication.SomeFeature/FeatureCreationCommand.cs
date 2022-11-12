namespace SingleFilePublication.SomeFeature;

public class FeatureCreationCommand
{
    public FeatureCreationCommand(string name) => this.Name = name;

    public string Name { get; }
}
