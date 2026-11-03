using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Smusdi.Core;

internal static class ReverseProxySetup
{
    private const string ForwardedHeadersEnabledEnvironmentVariable = "ASPNETCORE_FORWARDEDHEADERS_ENABLED";

    public static void ConfigureOpenApi(IServiceCollection services, ReverseProxyOptions options)
    {
        var serverUrl = GetOpenApiServerUrl(options);
        if (serverUrl is null)
        {
            return;
        }

        services.ConfigureAll<OpenApiOptions>(openApiOptions =>
            openApiOptions.AddDocumentTransformer((document, _, _) =>
            {
                document.Servers = [new OpenApiServer { Url = serverUrl }];
                return Task.CompletedTask;
            }));
    }

    public static void ConfigureMiddleware(WebApplication webApplication, ReverseProxyOptions options)
    {
        if (string.Equals(
            Environment.GetEnvironmentVariable(ForwardedHeadersEnabledEnvironmentVariable),
            bool.TrueString,
            StringComparison.OrdinalIgnoreCase))
        {
            var forwardedHeadersOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
                    | ForwardedHeaders.XForwardedProto
                    | ForwardedHeaders.XForwardedHost,
            };
            forwardedHeadersOptions.KnownIPNetworks.Clear();
            forwardedHeadersOptions.KnownProxies.Clear();
            webApplication.UseForwardedHeaders(forwardedHeadersOptions);
        }

        var basePath = GetBasePath(options.BasePath);
        if (basePath.HasValue)
        {
            webApplication.UsePathBase(basePath);
            webApplication.Use(async (context, next) =>
            {
                var existingPathBase = context.Request.PathBase.Value;
                if (existingPathBase is null
                    || !existingPathBase.EndsWith(basePath.Value, StringComparison.OrdinalIgnoreCase))
                {
                    context.Request.PathBase = context.Request.PathBase.Add(basePath);
                }

                await next();
            });
        }
    }

    private static string? GetOpenApiServerUrl(ReverseProxyOptions options)
    {
        var basePath = GetBasePath(options.BasePath).Value;
        var server = options.Server?.Trim().TrimEnd('/');
        if (string.IsNullOrWhiteSpace(server))
        {
            return basePath;
        }

        if (!Uri.TryCreate(server, UriKind.Absolute, out var serverUri)
            || (serverUri.Scheme != Uri.UriSchemeHttp && serverUri.Scheme != Uri.UriSchemeHttps)
            || !string.IsNullOrEmpty(serverUri.Query)
            || !string.IsNullOrEmpty(serverUri.Fragment))
        {
            throw new InvalidOperationException(
                "The smusdi:reverseProxy:server setting must be an absolute HTTP or HTTPS URL without a query or fragment.");
        }

        return $"{server}{basePath}";
    }

    private static PathString GetBasePath(string? configuredBasePath)
    {
        if (string.IsNullOrWhiteSpace(configuredBasePath) || configuredBasePath.Trim() == "/")
        {
            return PathString.Empty;
        }

        var basePath = $"/{configuredBasePath.Trim().Trim('/')}";
        if (basePath.Contains('\\') || basePath.Contains('?') || basePath.Contains('#'))
        {
            throw new InvalidOperationException(
                "The smusdi:reverseProxy:basePath setting must be a URL path without backslashes, a query, or a fragment.");
        }

        return new PathString(basePath);
    }
}
