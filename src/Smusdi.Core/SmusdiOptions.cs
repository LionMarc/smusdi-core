namespace Smusdi.Core;

public class SmusdiOptions
{
    public bool? NoVersioning { get; set; }

    public static SmusdiOptions? GetSmusdiOptions(IConfiguration configuration)
    {
        SmusdiOptions? smusdiOptions = null;
        if (configuration.GetSection("smusdi").Exists())
        {
            smusdiOptions = new SmusdiOptions();
            configuration.Bind("smusdi", smusdiOptions);
        }

        return smusdiOptions;
    }
}
