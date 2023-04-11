namespace Smusdi.Core;

public sealed class StaticSite
{
    public string Folder { get; set; } = string.Empty;

    public string RequestPath { get; set; } = string.Empty;

    public bool ServeUnknownFileTypes { get; set; }
}
