using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Time.Testing;

namespace Smusdi.Testing.Clock;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClockMock(this IServiceCollection services)
    {
        services.Replace(new ServiceDescriptor(typeof(TimeProvider), new FakeTimeProvider()));
        return services;
    }
}
