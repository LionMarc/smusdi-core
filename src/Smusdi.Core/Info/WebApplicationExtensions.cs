using System.IO.Abstractions;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Smusdi.Core.Info;

public static class WebApplicationExtensions
{
    public static WebApplication UseInfoEndpoint(this WebApplication webApplication)
    {
        webApplication.MapGet("/info", (IFileSystem fileSystem) =>
        {
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

            var customInfosFolder = Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiCustomInfoEnvVar);

            if (!string.IsNullOrWhiteSpace(customInfosFolder) && fileSystem.Directory.Exists(customInfosFolder))
            {
                foreach (var info in fileSystem.Directory.GetFiles(customInfosFolder, "*.json"))
                {
                    jsonObject.Add(Path.GetFileNameWithoutExtension(info), JsonObject.Parse(fileSystem.File.ReadAllText(info)));
                }
            }

            return jsonObject;
        });

        return webApplication;
    }
}
