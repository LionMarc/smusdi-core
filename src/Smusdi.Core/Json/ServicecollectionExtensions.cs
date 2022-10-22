using Microsoft.AspNetCore.Mvc;

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
}
