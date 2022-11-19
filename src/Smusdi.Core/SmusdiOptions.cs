namespace Smusdi.Core;

public sealed class SmusdiOptions
{
    public bool? NoVersioning { get; set; }

    /// <summary>
    /// Gets or sets a list of assembly names to allow scanning assemblies when publishing app as single file.
    /// </summary>
    public ICollection<string> AssemblyNames { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the list of static sites to be served by the service.
    /// </summary>
    public ICollection<StaticSite> StaticSites { get; set; } = new List<StaticSite>();

    public static SmusdiOptions GetSmusdiOptions(IConfiguration configuration)
    {
        SmusdiOptions? smusdiOptions = null;
        if (configuration.GetSection("smusdi").Exists())
        {
            smusdiOptions = new SmusdiOptions();
            configuration.Bind("smusdi", smusdiOptions);
        }

        return smusdiOptions ?? new SmusdiOptions();
    }
}
