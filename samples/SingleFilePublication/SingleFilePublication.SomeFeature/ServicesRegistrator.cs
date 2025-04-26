using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Extensibility;

namespace SingleFilePublication.SomeFeature;

internal class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFeatureRepository, FeatureRepository>()
            .AddScoped<IValidator<FeatureCreationCommand>, FeatureCreationCommandValidator>();
        return services;
    }
}
