using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Extensibility;
using Smusdi.PostgreSQL;

namespace PostgreSqlMigration;

internal class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MigrationDbContext>(optionsBuilder => optionsBuilder
            .UseNpgsql(configuration.GetConnectionString(Constants.ConnectionStringName), x => x.MigrationsHistoryTable("__ef_migrations_history", "test"))
            .UseSnakeCaseNamingConvention());

        return services;
    }
}
