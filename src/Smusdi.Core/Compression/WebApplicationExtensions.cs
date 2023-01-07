namespace Smusdi.Core.Compression;

public static class WebApplicationExtensions
{
    public static WebApplication UseResponseCompression(this WebApplication webApplication, SmusdiOptions smusdiOptions)
    {
        if (!smusdiOptions.CompressionDisabled)
        {
            webApplication.UseResponseCompression();
        }

        return webApplication;
    }
}
