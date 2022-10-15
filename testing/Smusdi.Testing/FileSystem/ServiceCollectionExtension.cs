using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Smusdi.Testing.FileSystem;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFileSystemMock(this IServiceCollection services)
    {
        services.Replace(new ServiceDescriptor(typeof(IFileSystem), typeof(MockFileSystem), ServiceLifetime.Singleton));
        return services;
    }
}
