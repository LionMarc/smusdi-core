using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core.Pipeline;
using Smusdi.Extensibility;

namespace Smusdi.Core.Specs.Pipeline;

public sealed class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeyedScoped<IPipelineStage<PipelineTestingContext>, Keyed1Step>("keyed1")
            .AddKeyedScoped<IPipelineStage<PipelineTestingContext>, Keyed2Step>("keyed2")
            .AddKeyedScoped<IPipelineStage<PipelineTestingContext>, Keyed3Step>("keyed3")
            .AddKeyedScoped<IPipelineStage<PipelineTestingContext>, Keyed4Step>("keyed4");

        return services;
    }
}
