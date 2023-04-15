using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Smusdi.PosgreSQL.Audit;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAudit(this IServiceCollection services, string databaseSchema, string? connectionString)
    {
        services
            .AddSingleton<IAuditDbContextSchemaNameProvider>(new AuditDbContextSchemaNameProvider(databaseSchema))
            .AddDbContext<AuditDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention())
            .AddScoped<IAuditService, AuditService>();

        return services;
    }
}
