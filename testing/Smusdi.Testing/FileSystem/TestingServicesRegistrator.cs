using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Smusdi.Testing.FileSystem;

public sealed class TestingServicesRegistrator : ITestingServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var useFileSystemMock = configuration.GetValue<bool>(Constants.UseFileSystemMockTag, false);
        if (useFileSystemMock)
        {
            services.AddFileSystemMock();
        }

        return services;
    }
}
