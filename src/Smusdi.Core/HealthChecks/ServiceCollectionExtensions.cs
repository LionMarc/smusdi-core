namespace Smusdi.Core.HealthChecks;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks();

        return services;
    }
}
