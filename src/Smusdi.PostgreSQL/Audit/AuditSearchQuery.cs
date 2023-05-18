namespace Smusdi.PostgreSQL.Audit;

public sealed class AuditSearchQuery
{
    public IEnumerable<string>? Types { get; set; }

    public string? ObjectType { get; set; }

    public IEnumerable<string>? ObjectIds { get; set; }
}
