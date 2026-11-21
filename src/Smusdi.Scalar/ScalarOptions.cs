using Microsoft.Extensions.Configuration;

namespace Smusdi.Scalar;

public sealed class ScalarOptions
{
    public const string ConfigurationSection = "scalar";

    public bool EnableAgent { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ContactEmail { get; set; }

    public static ScalarOptions GetScalarOptions(IConfiguration configuration) =>
        configuration.GetSection(ConfigurationSection).Get<ScalarOptions>() ?? new ScalarOptions();
}
