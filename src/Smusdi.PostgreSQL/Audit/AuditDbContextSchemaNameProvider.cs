using Microsoft.EntityFrameworkCore;
using Smusdi.PosgreSQL.ValueConverters;

namespace Smusdi.PosgreSQL.Audit;

public sealed class AuditDbContextSchemaNameProvider : IAuditDbContextSchemaNameProvider
{
    public AuditDbContextSchemaNameProvider(string schemaName) => this.SchemaName = schemaName;

    public string SchemaName { get; }
}
