namespace SingleFilePublication.SomeFeature;

internal class FeatureRepository : IFeatureRepository
{
    private static readonly List<Feature> Features = new();
    private static int nextId = 0;

    public Feature Add(FeatureCreationCommand command)
    {
#pragma warning disable S2696 // Instance members should not write to "static" fields
        var id = nextId++;
#pragma warning restore S2696 // Instance members should not write to "static" fields
        var feature = new Feature(id, command.Name);
        Features.Add(feature);
        return feature;
    }

    public IEnumerable<Feature> GetAll() => Features.ToList();

    public Feature? GetById(int featureId) => Features.Find(f => f.Id == featureId);
}
