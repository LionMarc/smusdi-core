namespace Smusdi.Core.Configuration;

public class EnvironmentVariablesExpanderProvider : ConfigurationProvider, IDisposable
{
    private readonly Lazy<ConfigurationRoot> configurationRoot;

    public EnvironmentVariablesExpanderProvider(IConfigurationBuilder builder)
    {
        this.configurationRoot = new Lazy<ConfigurationRoot>(() =>
        {
            var filteredProviders = builder.Build().Providers?
                .Where(p => p.GetType() != typeof(EnvironmentVariablesExpanderProvider))
                .ToList();
            return new ConfigurationRoot(filteredProviders);
        });
    }

    public override bool TryGet(string key, out string value)
    {
        var rootValue = this.configurationRoot.Value[key];
        value = !string.IsNullOrWhiteSpace(rootValue) ? Environment.ExpandEnvironmentVariables(rootValue) : string.Empty;

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
