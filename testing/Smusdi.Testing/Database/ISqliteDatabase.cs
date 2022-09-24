using Microsoft.EntityFrameworkCore;

namespace Smusdi.Testing.Database;

public interface ISqliteDatabase
{
    string FilePath { get; }

    DbContext GetMigrationContext(IServiceProvider serviceProvider);
}
