namespace Smusdi.Core.Helpers;

public static class EnvFileHelper
{
    public static void ReadEnvFileIfExists()
    {
        var envFile = Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiEnvFilePath) ?? SmusdiConstants.DefaultEnvFile;
        if (!string.IsNullOrWhiteSpace(envFile) && File.Exists(envFile))
        {
            foreach (var line in File.ReadAllLines(envFile))
            {
                var parts = line.Split('=');
                if (parts.Length >= 2)
                {
                    var value = Environment.ExpandEnvironmentVariables(string.Join('=', parts.Skip(1)));
                    Environment.SetEnvironmentVariable(parts[0], value);
                }
            }
        }
    }
}
