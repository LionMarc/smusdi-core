using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Smusdi.Testing.Database;

public class SqliteDatabase<TMigrationContext> : ISqliteDatabase
    where TMigrationContext : DbContext
{
    public SqliteDatabase() => this.FilePath = $"smusdi-{Guid.NewGuid()}.sqlite";

    public string FilePath { get; }

    public DbContext GetMigrationContext(IServiceProvider serviceProvider) => serviceProvider.GetRequiredService<TMigrationContext>();
}
