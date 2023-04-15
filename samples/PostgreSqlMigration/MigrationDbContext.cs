using Microsoft.EntityFrameworkCore;
using Smusdi.PosgreSQL.Audit;

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

        AuditDbContext.CreateTables(modelBuilder);
    }
}
