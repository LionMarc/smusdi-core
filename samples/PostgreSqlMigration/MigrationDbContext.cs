using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smusdi.PostgreSQL;
using Smusdi.PostgreSQL.Audit;
using Smusdi.PostgreSQL.ValueConverters;

namespace PostgreSqlMigration;

public class MigrationDbContext : DbContext
{
    private readonly IConfiguration configuration;

    public MigrationDbContext(DbContextOptions<MigrationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        this.configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(this.configuration.GetPostgreSqlSchema());

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
