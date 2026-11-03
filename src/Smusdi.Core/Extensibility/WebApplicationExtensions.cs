using Smusdi.Extensibility;

namespace Smusdi.Core.Extensibility;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyCustomConfigurators(this WebApplication webApplication)
    {
        var collection = new ServiceCollection();
        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(webApplication.Configuration);

        collection.Scan(scan => scan
            .FromAssembliesOrApplicationDependencies(smusdiOptions)
            .AddClasses(c => c.AssignableTo<IWebApplicationConfigurator>(), false)
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        foreach (var configurator in collection.BuildServiceProvider().GetServices<IWebApplicationConfigurator>())
        {
            configurator.Configure(webApplication);
        }

        return webApplication;
    }
}
