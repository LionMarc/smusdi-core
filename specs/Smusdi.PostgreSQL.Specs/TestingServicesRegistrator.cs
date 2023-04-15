using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostgreSqlMigration;
using Smusdi.Testing;

namespace Smusdi.PostgreSQL.Specs;

internal class TestingServicesRegistrator : ITestingServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MigrationDbContext>(optionsBuilder => optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("postgresql"), x => x.MigrationsHistoryTable("__ef_migrations_history", MigrationDbContext.Schema))
            .UseSnakeCaseNamingConvention());
        services.AddDatabaseMigrator<MigrationDbContext>();

        return services;
    }
}
