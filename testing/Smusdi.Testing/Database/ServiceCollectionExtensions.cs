using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Smusdi.Testing.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlite<TMigrationContext>(this IServiceCollection services)
        where TMigrationContext : DbContext
    {
        services.TryAddSingleton<ISqliteDatabase, SqliteDatabase<TMigrationContext>>();

        services.AddEntityFrameworkSqlite();

        services.AddSqliteContext<TMigrationContext>();

        return services;
    }

    public static IServiceCollection AddSqliteContext<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        var dbContextOptions = services.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<TContext>));
        if (dbContextOptions != null)
        {
            services.Remove(dbContextOptions);
        }

        services.AddDbContext<TContext>(
            (s, options) => options.UseSqlite($"Data source={s.GetRequiredService<ISqliteDatabase>().FilePath}"),
            ServiceLifetime.Scoped,
            ServiceLifetime.Singleton);

        return services;
    }
}
