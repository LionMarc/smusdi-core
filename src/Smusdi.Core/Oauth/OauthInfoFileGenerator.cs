using System.IO.Abstractions;
using Serilog;
using Smusdi.Core.Info;
using Smusdi.Core.Json;
using Smusdi.Extensibility;

namespace Smusdi.Core.Oauth;

public sealed class OauthInfoFileGenerator : IBeforeRun
{
    private readonly IConfiguration configuration;
    private readonly IFileSystem fileSystem;
    private readonly IJsonSerializer jsonSerializer;
    private readonly ILogger logger;

    public OauthInfoFileGenerator(IConfiguration configuration, IFileSystem fileSystem, IJsonSerializer jsonSerializer, ILogger logger)
    {
        this.configuration = configuration;
        this.fileSystem = fileSystem;
        this.jsonSerializer = jsonSerializer;
        this.logger = logger;
    }

    public Task Execute()
    {
        var options = OauthOptions.GetOauthOptions(this.configuration);
        if (string.IsNullOrWhiteSpace(options?.Authority))
        {
            return Task.CompletedTask;
        }

        var infoFolder = CustomInfoHelpers.GetInfoFolder();
        if (string.IsNullOrWhiteSpace(infoFolder))
        {
            return Task.CompletedTask;
        }

        this.logger.Information("Generating info file for oauth.");
        this.fileSystem.Directory.CreateDirectory(infoFolder);
        var oauthInfo = new OauthInfo()
        {
            Authority = options.Authority,
        };

        var outputPath = Path.Combine(infoFolder, "oauth.json");
        if (this.fileSystem.File.Exists(outputPath))
        {
            this.fileSystem.File.Delete(outputPath);
        }

        var serialized = this.jsonSerializer.Serialize(oauthInfo);
        this.fileSystem.File.WriteAllText(outputPath, serialized);
        return Task.CompletedTask;
    }

    private sealed class OauthInfo
    {
        public string Authority { get; set; } = string.Empty;
    }
}
