using Smusdi.Extensibility;

namespace Smusdi.Core.Configuration;

public sealed class EnvironmentVariablesExpanderProvider : ConfigurationProvider, IDisposable
{
    private readonly Lazy<ConfigurationRoot> configurationRoot;

    public EnvironmentVariablesExpanderProvider(IConfigurationBuilder builder)
    {
        this.configurationRoot = new Lazy<ConfigurationRoot>(() =>
        {
            var filteredProviders = (builder.Build().Providers ?? Enumerable.Empty<IConfigurationProvider>())
                .Where(p => p is not EnvironmentVariablesExpanderProvider)
                .ToList();
            return new ConfigurationRoot(filteredProviders);
        });
    }

    public override bool TryGet(string key, out string value)
    {
        var rootValue = this.configurationRoot.Value[key];
        if (string.IsNullOrEmpty(rootValue))
        {
            value = string.Empty;
            return false;
        }

        value = Environment.ExpandEnvironmentVariables(rootValue);
        if (value != rootValue && value.IndexOf("%") != -1 && bool.TryParse(Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiExpandEnvTwiceEnvVar), out var mustTryExpandingAgain) && mustTryExpandingAgain)
        {
            value = Environment.ExpandEnvironmentVariables(value);
        }

        return !string.IsNullOrEmpty(value);
    }

    public void Dispose()
    {
        if (this.configurationRoot.IsValueCreated)
        {
            this.configurationRoot.Value.Dispose();
        }
    }
}
