using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using RichardSzalay.MockHttp;

namespace Smusdi.Testing.Http;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMockedHttpClient<TClient, TImplementation>(this IServiceCollection services)
        where TClient : class
        where TImplementation : class, TClient
    {
        services.TryAddSingleton<MockHttpMessageHandler>();
        var builder = services.AddHttpClient<TClient, TImplementation>();
        builder.Services.Configure<HttpClientFactoryOptions>(builder.Name, options =>
        {
            options.HttpMessageHandlerBuilderActions.Clear();
            options.HttpMessageHandlerBuilderActions.Add(b => b.PrimaryHandler = b.Services.GetService<MockHttpMessageHandler>()!);
        });

        return services;
    }
}
