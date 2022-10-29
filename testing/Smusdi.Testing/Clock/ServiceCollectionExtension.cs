using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Smusdi.Core.Helpers;

namespace Smusdi.Testing.Clock;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClockMock(this IServiceCollection services)
    {
        services.Replace(new ServiceDescriptor(typeof(IClock), typeof(ClockMock), ServiceLifetime.Singleton));
        return services;
    }
}
