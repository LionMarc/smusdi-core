using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Smusdi.Core.Extensibility;

namespace Smusdi.PostgreSQL;

public class DatabaseMigrator<T> : IBeforeRun
    where T : DbContext
{
    private readonly T migrationContext;
    private readonly ILogger logger;

    public DatabaseMigrator(T migrationContext, ILogger logger)
    {
        this.migrationContext = migrationContext;
        this.logger = logger;
    }

    public async Task Execute()
    {
        var migrateDatabaseEnvValue = Environment.GetEnvironmentVariable(EnvironmentVariableNames.MigrateDatabse);
        if (bool.TryParse(migrateDatabaseEnvValue, out var migrateDatabase) && migrateDatabase)
        {
            this.logger.Information("Database migration is enabled.");
            var connectionString = Regex.Replace(this.migrationContext.Database.GetDbConnection().ConnectionString, ";Password=[^;]+;", ";Password=MASKED;");
            this.logger.Information($"Migrating database {connectionString}...");
            await this.migrationContext.Database.MigrateAsync();
        }
        else
        {
            this.logger.Information("Database migration is disabled.");
        }
    }
}
