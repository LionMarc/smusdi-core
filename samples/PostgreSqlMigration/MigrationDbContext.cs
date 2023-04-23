using Microsoft.EntityFrameworkCore;
using Smusdi.PostgreSQL.Audit;
using Smusdi.PostgreSQL.ValueConverters;

namespace PostgreSqlMigration;

public class MigrationDbContext : DbContext
{
    public const string Schema = "smusdi";

    public MigrationDbContext(DbContextOptions<MigrationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<JobDao>()
            .ToTable("jobs")
            .HasKey(j => j.Id);

        modelBuilder.Entity<JobDao>()
            .Property(j => j.UtcStartTimestamp)
            .HasConversion<UtcDateTimeConverter>();

        modelBuilder.Entity<JobDao>()
            .Property(j => j.UtcEndTimestamp)
            .HasConversion<NullableUtcDateTimeConverter>();

        AuditDbContext.CreateTables(modelBuilder);
    }
}
