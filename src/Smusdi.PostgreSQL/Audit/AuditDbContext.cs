using Microsoft.EntityFrameworkCore;
using Smusdi.PostgreSQL.ValueConverters;

namespace Smusdi.PostgreSQL.Audit;

public class AuditDbContext : DbContext
{
    private readonly IAuditDbContextSchemaNameProvider schemaNameProvider;

    public AuditDbContext(DbContextOptions<AuditDbContext> options, IAuditDbContextSchemaNameProvider schemaNameProvider)
    : base(options)
    {
        this.schemaNameProvider = schemaNameProvider;
    }

    public static void CreateTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditRecordDao>()
            .ToTable("audit_records")
        .HasKey(a => a.Id);

        modelBuilder.Entity<AuditRecordDao>()
            .HasIndex(a => a.ObjectId);

        modelBuilder.Entity<AuditRecordDao>()
            .HasIndex(a => a.ObjectType);

        modelBuilder.Entity<AuditRecordDao>()
            .Property(a => a.UtcCreationTimestamp)
            .HasConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(this.schemaNameProvider.SchemaName);
        CreateTables(modelBuilder);
    }
}
