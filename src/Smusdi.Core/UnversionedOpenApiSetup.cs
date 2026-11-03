namespace Smusdi.Core;

internal static class UnversionedOpenApiSetup
{
    public static void AddServices(IServiceCollection services) => services.AddOpenApi();

    public static void MapEndpoint(WebApplication webApplication) => webApplication.MapOpenApi();
}
