using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Testing;
using Smusdi.Testing.Clock;

namespace Smusdi.Core.Specs.Clock;

internal class TestingServicesRegistrator : ITestingServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddClockMock();

        return services;
    }
}
