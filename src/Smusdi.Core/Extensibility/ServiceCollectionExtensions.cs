namespace Smusdi.Core.Extensibility;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices<T>(this IServiceCollection services, IConfiguration configuration)
        where T : IBaseServicesRegistrator
    {
        var collection = new ServiceCollection();
        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(configuration);
        collection.Scan(scan => scan
            .FromAssembliesOrApplicationDependencies(smusdiOptions)
            .AddClasses(c => c.AssignableTo<T>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        foreach (var registrator in collection.BuildServiceProvider().GetServices<T>())
        {
            registrator.Add(services, configuration);
        }

        return services;
    }
}
