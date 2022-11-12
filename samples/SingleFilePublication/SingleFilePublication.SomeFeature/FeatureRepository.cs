namespace SingleFilePublication.SomeFeature;

internal class FeatureRepository : IFeatureRepository
{
    private static readonly List<Feature> Features = new List<Feature>();
    private static int nextId = 1;

    public Feature Add(FeatureCreationCommand command)
    {
        var feature = new Feature(nextId++, command.Name);
        Features.Add(feature);
        return feature;
    }

    public IEnumerable<Feature> GetAll() => Features.ToList();

    public Feature? GetById(int featureId) => Features.Find(f => f.Id == featureId);
}
