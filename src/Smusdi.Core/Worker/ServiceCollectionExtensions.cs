using Smusdi.Core.Extensibility;

namespace Smusdi.Core.Worker;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkerTasks(this IServiceCollection services, IConfiguration configuration)
    {
        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(configuration);
        services.Scan(scan => scan
            .FromAssembliesOrApplicationDependencies(smusdiOptions)
            .AddClasses(c => c.AssignableTo<IWorkerTask>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
