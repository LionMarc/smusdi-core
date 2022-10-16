namespace Smusdi.Core.Extensibility;

internal static class WebApplicationExtensions
{
    public static WebApplication ApplyCustomConfigurators(this WebApplication webApplication)
    {
        var collection = new ServiceCollection();

        collection.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo<IWebApplicationConfigurator>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        foreach (var configurator in collection.BuildServiceProvider().GetServices<IWebApplicationConfigurator>())
        {
            configurator.Configure(webApplication);
        }

        return webApplication;
    }
}
