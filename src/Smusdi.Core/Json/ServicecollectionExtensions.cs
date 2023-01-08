using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Smusdi.Core.Json;

public static class ServicecollectionExtensions
{
    public static IServiceCollection AddPolymorphicConverter<T>(this IServiceCollection services, PolymorphicConverterOptions polymorphicConverterOptions)
    {
        services.Configure((JsonOptions options) =>
        {
            options.JsonSerializerOptions.Converters.Add(new PolymorphicConverter<T>(polymorphicConverterOptions));
        });

        return services;
    }

    public static IServiceCollection AddJsonSerializerWithJsonOptions(this IServiceCollection services)
    {
        services.TryAddSingleton<IJsonSerializer, JsonSerializerUsingJsonOptions>();

        return services;
    }
}
