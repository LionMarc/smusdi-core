using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Testing;
using Smusdi.Testing.FileSystem;

namespace Smusdi.Core.Specs;

internal class TestingServicesRegistrator : ITestingServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFileSystemMock();

        return services;
    }
}
