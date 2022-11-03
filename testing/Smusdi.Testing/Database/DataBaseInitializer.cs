using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace Smusdi.Testing.Database;

[Binding]
public sealed class DataBaseInitializer : IDisposable
{
    private readonly SmusdiTestingService smusdiTestingService;
    private string? filePath;

    public DataBaseInitializer(SmusdiTestingService smusdiTestingService) => this.smusdiTestingService = smusdiTestingService;

    [Given(@"the database initialized")]
    public void GivenTheDatabaseInitialized()
    {
        var sqliteDatabase = this.smusdiTestingService.GetService<ISqliteDatabase>();
        if (sqliteDatabase == null)
        {
            return;
        }

        this.filePath = sqliteDatabase.FilePath;

        var provider = this.smusdiTestingService.GetService<IServiceProvider>();
        if (provider == null)
        {
            return;
        }

        using var scope = provider.CreateScope();
        using var context = sqliteDatabase.GetMigrationContext(scope.ServiceProvider);
        var databaseCreator = context.GetService<IRelationalDatabaseCreator>();
        databaseCreator.CreateTables();
    }

    public void Dispose()
    {
        if (!string.IsNullOrWhiteSpace(this.filePath) && File.Exists(this.filePath))
        {
            try
            {
                // https://github.com/dotnet/efcore/issues/27139
                SqliteConnection.ClearAllPools();
                File.Delete(this.filePath);
            }
            catch
            {
                // do nothing
            }
        }
    }
}
