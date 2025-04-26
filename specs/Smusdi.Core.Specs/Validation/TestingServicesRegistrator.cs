using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Testing;

namespace Smusdi.Core.Specs.Validation;

internal class TestingServicesRegistrator : ITestingServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidator<Project>, ProjectValidator>()
            .AddScoped<IValidator<Target>, TargetValidator>();

        return services;
    }
}
