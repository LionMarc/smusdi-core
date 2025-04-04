using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core.Json;
using Smusdi.Extensibility;

namespace Smusdi.Sample.Controllers.Json;

public class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var options = new PolymorphicConverterOptions("type", new Dictionary<string, Type>());
        options.SubTypes.Add("ruleCreationCommand", typeof(RuleCreationCommand));
        options.SubTypes.Add("ruleUpdateCommand", typeof(RuleUpdateCommand));

        services.AddPolymorphicConverter<Command>(options);

        return services;
    }
}
