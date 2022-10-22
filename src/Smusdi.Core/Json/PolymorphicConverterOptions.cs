namespace Smusdi.Core.Json;

public class PolymorphicConverterOptions
{
    public PolymorphicConverterOptions(string discriminator, IDictionary<string, Type> subTypes)
    {
        this.Discriminator = discriminator;
        this.SubTypes = subTypes;
    }

    public string Discriminator { get; }

    public IDictionary<string, Type> SubTypes { get; }
}
