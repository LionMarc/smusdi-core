using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core.Extensibility;
using Smusdi.Core.Helpers;
using Smusdi.Core.Json;

namespace Smusdi.Core.Specs.Json;

public class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var options = new PolymorphicConverterOptions(nameof(Workflow.Type).ToCamelCase(), new Dictionary<string, Type>());
        options.SubTypes.Add(WorkflowType.Standard.ToString(), typeof(StandardWorklflow));
        options.SubTypes.Add(WorkflowType.Simplified.ToString(), typeof(SimplifiedWorklflow));
        services.AddPolymorphicConverter<Workflow>(options);

        options = new PolymorphicConverterOptions(nameof(Stage.Type).ToCamelCase(), new Dictionary<string, Type>());
        options.SubTypes.Add(StageType.Build.ToString(), typeof(BuildStage));
        options.SubTypes.Add(StageType.Test.ToString(), typeof(TestStage));
        services.AddPolymorphicConverter<Stage>(options);

        services.AddJsonSerializerWithJsonOptions();

        return services;
    }
}
