namespace Smusdi.PostgreSQL.Audit;

public sealed class AuditDbContextSchemaNameProvider : IAuditDbContextSchemaNameProvider
{
    public AuditDbContextSchemaNameProvider(string schemaName) => this.SchemaName = schemaName;

    public string SchemaName { get; }
}
