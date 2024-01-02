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
        var connectionString = configuration.GetConnectionString(Constants.ConnectionStringName);
        services.AddDbContext<MigrationDbContext>(optionsBuilder => optionsBuilder
            .UseNpgsql(connectionString, x => x.MigrationsHistoryTable("__ef_migrations_history", configuration.GetPostgreSqlSchema()))
            .UseSnakeCaseNamingConvention());
        services.AddDatabaseMigrator<MigrationDbContext>();

        services.AddAudit(connectionString);

        services.AddClockMock();

        return services;
    }
}
