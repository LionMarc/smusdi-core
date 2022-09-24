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

        using var scope = this.smusdiTestingService.GetService<IServiceProvider>().CreateScope();
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
                File.Delete(this.filePath);
            }
            catch
            {
                // do nothing
            }
        }
    }
}
