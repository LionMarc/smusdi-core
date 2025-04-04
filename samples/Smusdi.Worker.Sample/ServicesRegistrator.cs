using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core.Helpers;
using Smusdi.Extensibility;

namespace Smusdi.Worker.Sample;

internal sealed class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClientWithClientCredentials<ISampleService, SampleService>(
            client =>
            {
#pragma warning disable S1075 // URIs should not be hardcoded
                client.BaseAddress = new Uri("http://localhost:5100/v1/");
#pragma warning restore S1075 // URIs should not be hardcoded
            },
            "PSG");

        return services;
    }
}
