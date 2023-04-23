namespace PostgreSqlMigration;

public sealed class JobDao
{
    public int Id { get; set; }

    public DateTime UtcStartTimestamp { get; set; }

    public DateTime? UtcEndTimestamp { get; set; }
}
