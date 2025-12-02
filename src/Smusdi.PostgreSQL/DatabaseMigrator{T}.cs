using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Smusdi.Extensibility;

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
            var connectionString = this.migrationContext.Database.GetDbConnection().ConnectionString;
            var maskedConnectionString = MaskSensitiveInfo(connectionString);
            this.logger.Information("Migrating database {ConnectionString}...", maskedConnectionString);
            await this.migrationContext.Database.MigrateAsync();
        }
        else
        {
            this.logger.Information("Database migration is disabled.");
        }
    }

    /// <summary>
    /// Masks sensitive information (Password, User ID, Username, AccountKey, etc.) in a connection string.
    /// </summary>
    /// <param name="connectionString">The original connection string.</param>
    /// <returns>The masked connection string, safe for logging.</returns>
    private static string MaskSensitiveInfo(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        // Mask common secret fields (add more as needed)
        string[] sensitiveKeys = new[] { "Password", "Pwd", "User Id", "Username", "AccountKey", "Secret" };
        foreach (var key in sensitiveKeys)
        {
            // Match both ';Key=Value;' and ';Key=Value' at end
            var regex = new Regex($@"(?i)(;{Regex.Escape(key)}=)[^;]*", RegexOptions.Compiled);
            connectionString = regex.Replace(connectionString, $" ;{key}=MASKED");
        }

        return connectionString;
    }
}
