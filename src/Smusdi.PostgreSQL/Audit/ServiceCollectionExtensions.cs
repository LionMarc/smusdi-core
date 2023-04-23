using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Smusdi.PostgreSQL.Audit;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAudit(this IServiceCollection services, string? connectionString)
    {
        services
            .AddDbContext<AuditDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention())
            .AddScoped<IAuditService, AuditService>();

        return services;
    }
}
