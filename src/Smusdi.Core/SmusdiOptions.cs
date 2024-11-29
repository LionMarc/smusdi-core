using System.IO.Compression;

namespace Smusdi.Core;

public sealed class SmusdiOptions
{
    public const string SectionName = "smusdi";

    public const int MaxSizeOf1Giga = 1024 * 1024 * 1024;

    public const int MaxSizeOf1Mega = 1024 * 1024;

    public static string ServiceName => Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiServiceNameEnvVar) ?? "smusdi";

    public bool? NoVersioning { get; set; }

    /// <summary>
    /// Gets or sets a list of assembly names to allow scanning assemblies when publishing app as single file.
    /// </summary>
    public ICollection<string> AssemblyNames { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the list of static sites to be served by the service.
    /// </summary>
    public ICollection<StaticSite> StaticSites { get; set; } = new List<StaticSite>();

    /// <summary>
    /// Gets or sets a value indicating whether or not the response compression is disabled.
    /// </summary>
    /// <remarks>By default, compression is enabled.</remarks>
    public bool CompressionDisabled { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether or not the response compression is disabled for https call.
    /// </summary>
    /// <remarks>By default, compression is enabled.</remarks>
    public bool CompressionDisabledForHttps { get; set; } = false;

    /// <summary>
    /// Gets or sets the compression level for BrotliCompressionProvider and GzipCompressionProvider.
    /// </summary>
    /// <remarks>Default value is <see cref="CompressionLevel.Fastest"/>.</remarks>
    public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

    public int MaxRequestBodySize { get; set; } = MaxSizeOf1Giga;

    public int MaxMultipartBodySize { get; set; } = MaxSizeOf1Giga;

    public int MaxMultipartValueSize { get; set; } = MaxSizeOf1Mega;

    public int MaxMultipartHeadersSize { get; set; } = MaxSizeOf1Mega;

    /// <summary>
    /// Gets or sets a value indicating whether or not a cache is used by the info endpoint.
    /// </summary>
    public bool InfoCacheDisabled { get; set; }

    public static SmusdiOptions GetSmusdiOptions(IConfiguration configuration)
    {
        SmusdiOptions? smusdiOptions = null;
        if (configuration.GetSection(SectionName).Exists())
        {
            smusdiOptions = new SmusdiOptions();
            configuration.Bind(SectionName, smusdiOptions);
        }

        return smusdiOptions ?? new SmusdiOptions();
    }
}
