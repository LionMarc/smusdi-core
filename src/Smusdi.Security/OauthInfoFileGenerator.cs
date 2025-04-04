using System.IO.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using Smusdi.Extensibility;

namespace Smusdi.Security;

public sealed class OauthInfoFileGenerator(IConfiguration configuration, IFileSystem fileSystem, ILogger logger) : IBeforeRun
{
    public Task Execute()
    {
        var options = OauthOptions.GetOauthOptions(configuration);
        if (string.IsNullOrWhiteSpace(options?.MainAuthority?.Url))
        {
            return Task.CompletedTask;
        }

        var infoFolder = CustomInfoHelpers.GetInfoFolder();
        if (string.IsNullOrWhiteSpace(infoFolder))
        {
            return Task.CompletedTask;
        }

        logger.Information("Generating info file for oauth.");
        fileSystem.Directory.CreateDirectory(infoFolder);
        var oauthInfo = new OauthInfo()
        {
            Authority = options.MainAuthority.Url,
        };

        var outputPath = Path.Combine(infoFolder, "oauth.json");
        if (fileSystem.File.Exists(outputPath))
        {
            fileSystem.File.Delete(outputPath);
        }

        var serializationOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var serialized = JsonSerializer.Serialize(oauthInfo, serializationOptions);
        fileSystem.File.WriteAllText(outputPath, serialized);
        return Task.CompletedTask;
    }

    private sealed class OauthInfo
    {
        public string Authority { get; set; } = string.Empty;
    }
}
