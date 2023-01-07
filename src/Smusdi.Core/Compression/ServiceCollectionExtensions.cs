using Microsoft.AspNetCore.ResponseCompression;

namespace Smusdi.Core.Compression;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddResponseCompression(this IServiceCollection services, SmusdiOptions smusdiOptions)
    {
        if (smusdiOptions.CompressionDisabled)
        {
            return services;
        }

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = !smusdiOptions.CompressionDisabledForHttps;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = smusdiOptions.CompressionLevel;
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = smusdiOptions.CompressionLevel;
        });

        return services;
    }
}
