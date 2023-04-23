using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smusdi.PostgreSQL.ValueConverters;

namespace Smusdi.PostgreSQL.Audit;

public class AuditDbContext : DbContext
{
    private readonly IConfiguration configuration;

    public AuditDbContext(DbContextOptions<AuditDbContext> options, IConfiguration configuration)
    : base(options)
    {
        this.configuration = configuration;
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
        modelBuilder.HasDefaultSchema(this.configuration.GetPostgreSqlSchema());
        CreateTables(modelBuilder);
    }
}
