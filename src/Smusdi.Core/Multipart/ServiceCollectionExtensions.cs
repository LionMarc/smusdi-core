using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Smusdi.Core.Multipart;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection SetupMultipartMaxSizes(this IServiceCollection services, SmusdiOptions smusdiOptions)
    {
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = smusdiOptions.MaxRequestBodySize;
        });

        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = smusdiOptions.MaxMultipartValueSize;
            options.MultipartBodyLengthLimit = smusdiOptions.MaxMultipartBodySize;
            options.MultipartHeadersLengthLimit = smusdiOptions.MaxMultipartHeadersSize;
        });

        return services;
    }
}
