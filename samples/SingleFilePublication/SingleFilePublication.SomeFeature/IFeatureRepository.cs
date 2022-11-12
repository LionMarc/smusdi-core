namespace SingleFilePublication.SomeFeature;

public interface IFeatureRepository
{
    Feature Add(FeatureCreationCommand command);

    IEnumerable<Feature> GetAll();

    Feature? GetById(int featureId);
}
