using System.IO.Abstractions;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Smusdi.Core.Info;

public static class WebApplicationExtensions
{
    public static WebApplication UseInfoEndpoint(this WebApplication webApplication)
    {
        webApplication.MapGet("/info", (IFileSystem fileSystem, IMemoryCache memoryCache, IOptions<SmusdiOptions> options) =>
        {
            const string Key = "smusdi_info_key";
            if (!options.Value.InfoCacheDisabled && memoryCache.TryGetValue(Key, out var result))
            {
                return result;
            }

            var version = Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiServiceVersionEnvVar);
            if (string.IsNullOrWhiteSpace(version))
            {
                version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
            }

            var jsonObject = new JsonObject
            {
                { "serviceName", Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiServiceNameEnvVar) },
                { "serviceVersion", version },
                { "environment", webApplication.Environment.EnvironmentName },
            };

            var customInfosFolder = CustomInfoHelpers.GetInfoFolder();

            if (!string.IsNullOrWhiteSpace(customInfosFolder) && fileSystem.Directory.Exists(customInfosFolder))
            {
                foreach (var info in fileSystem.Directory.GetFiles(customInfosFolder, "*.json"))
                {
                    jsonObject.Add(Path.GetFileNameWithoutExtension(info), JsonObject.Parse(fileSystem.File.ReadAllText(info)));
                }
            }

            if (!options.Value.InfoCacheDisabled)
            {
                memoryCache.Set(Key, jsonObject);
            }

            return jsonObject;
        }).ExcludeFromDescription();

        return webApplication;
    }
}
