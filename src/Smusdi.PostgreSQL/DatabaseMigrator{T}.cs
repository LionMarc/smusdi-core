using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Smusdi.Core.Extensibility;

namespace Smusdi.PostgreSQL;

public class DatabaseMigrator<T>(T migrationContext, ILogger logger) : IBeforeRun
    where T : DbContext
{
    private readonly T migrationContext = migrationContext;
    private readonly ILogger logger = logger;

    public async Task Execute()
    {
        var migrateDatabaseEnvValue = Environment.GetEnvironmentVariable(EnvironmentVariableNames.MigrateDatabse);
        if (bool.TryParse(migrateDatabaseEnvValue, out var migrateDatabase) && migrateDatabase)
        {
            this.logger.Information("Database migration is enabled.");
            var connectionString = Regex.Replace(this.migrationContext.Database.GetDbConnection().ConnectionString, ";Password=[^;]+;", ";Password=MASKED;");
            this.logger.Information("Migrating database {ConnectionString}...", connectionString);
            await this.migrationContext.Database.MigrateAsync();
        }
        else
        {
            this.logger.Information("Database migration is disabled.");
        }
    }
}
