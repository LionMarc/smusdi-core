using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostgreSqlMigration;
using Smusdi.PostgreSQL.Audit;
using Smusdi.Testing;
using Smusdi.Testing.Clock;

namespace Smusdi.PostgreSQL.Specs;

internal class TestingServicesRegistrator : ITestingServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgresql");
        services.AddDbContext<MigrationDbContext>(optionsBuilder => optionsBuilder
            .UseNpgsql(connectionString, x => x.MigrationsHistoryTable("__ef_migrations_history", MigrationDbContext.Schema))
            .UseSnakeCaseNamingConvention());
        services.AddDatabaseMigrator<MigrationDbContext>();

        services.AddAudit(MigrationDbContext.Schema, connectionString);

        services.AddClockMock();

        return services;
    }
}
