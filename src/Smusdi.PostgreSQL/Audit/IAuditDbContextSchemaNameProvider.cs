namespace Smusdi.PosgreSQL.Audit;

public interface IAuditDbContextSchemaNameProvider
{
    string SchemaName { get; }
}
