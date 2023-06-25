namespace Smusdi.Core.Info;

public static class CustomInfoHelpers
{
    public static string? GetInfoFolder() => Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiCustomInfoEnvVar);
}
