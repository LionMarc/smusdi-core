namespace Smusdi.PostgreSQL.Audit;

public interface IAuditDbContextSchemaNameProvider
{
    string SchemaName { get; }
}
