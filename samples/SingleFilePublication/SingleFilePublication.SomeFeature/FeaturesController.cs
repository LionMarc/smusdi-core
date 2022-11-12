using Microsoft.AspNetCore.Mvc;

namespace SingleFilePublication.SomeFeature;

[ApiController]
[Route("features")]
public class FeaturesController : ControllerBase
{
    private readonly IFeatureRepository featureRepository;

    public FeaturesController(IFeatureRepository featureRepository) => this.featureRepository = featureRepository;

    [HttpGet]
    public ActionResult<IEnumerable<Feature>> GetAll()
    {
        var features = this.featureRepository.GetAll();
        return this.Ok(features);
    }

    [HttpGet("{featureId}")]
    public ActionResult<Feature> GetById(int featureId)
    {
        var feature = this.featureRepository.GetById(featureId);
        if (feature == null)
        {
            return this.NotFound();
        }

        return this.Ok(feature);
    }

    [HttpPost]
    public ActionResult<Feature> Create([FromBody] FeatureCreationCommand command)
    {
        var feature = this.featureRepository.Add(command);
        return this.CreatedAtAction(
            nameof(this.GetById),
            new { featureId = feature.Id },
            feature);
    }
}
