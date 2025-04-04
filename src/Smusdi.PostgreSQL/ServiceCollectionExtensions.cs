using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Extensibility;

namespace Smusdi.PostgreSQL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseMigrator<T>(this IServiceCollection services)
        where T : DbContext
    {
        services.AddScoped<IBeforeRun, DatabaseMigrator<T>>();

        return services;
    }
}
