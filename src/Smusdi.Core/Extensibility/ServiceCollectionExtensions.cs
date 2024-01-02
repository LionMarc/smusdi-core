namespace Smusdi.Core.Extensibility;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices<T>(this IServiceCollection services, IConfiguration configuration)
        where T : IBaseServicesRegistrator
    {
        var registrators = ScrutorHelpers.GetImplementationsOf<T>(configuration);
        foreach (var registrator in registrators)
        {
            registrator.Add(services, configuration);
        }

        return services;
    }

    public static IServiceCollection AddBeforeRunImplementations(this IServiceCollection services, IConfiguration configuration)
    {
        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(configuration);
        services.Scan(scan => scan
            .FromAssembliesOrApplicationDependencies(smusdiOptions)
            .AddClasses(c => c.AssignableTo<IBeforeRun>().Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
