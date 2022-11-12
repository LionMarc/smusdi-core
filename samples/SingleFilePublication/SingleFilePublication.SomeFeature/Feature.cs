namespace SingleFilePublication.SomeFeature;

public class Feature
{
    public Feature(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public int Id { get; }

    public string Name { get; }
}
