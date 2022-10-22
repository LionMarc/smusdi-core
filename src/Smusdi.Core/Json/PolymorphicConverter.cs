using System.Text.Json;
using System.Text.Json.Serialization;

namespace Smusdi.Core.Json;

public class PolymorphicConverter<T> : JsonConverter<T>
{
    private readonly PolymorphicConverterOptions options;

    public PolymorphicConverter(PolymorphicConverterOptions options) => this.options = options;

    public override bool CanConvert(Type typeToConvert) =>
            typeof(T).IsAssignableFrom(typeToConvert);

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var cloned = reader;
        while (cloned.Read())
        {
            if (cloned.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            var propertyName = cloned.GetString();
            if (propertyName != this.options.Discriminator)
            {
                continue;
            }

            cloned.Read();
            var value = cloned.GetString();
            if (string.IsNullOrEmpty(value) || !this.options.SubTypes.TryGetValue(value, out var subType))
            {
                throw new JsonException($"No sub type defined with discrimator value {value} for base type {typeof(T)}");
            }

            var clonedOptions = new JsonSerializerOptions(options);
            clonedOptions.Converters.Remove(this);

            return (T?)JsonSerializer.Deserialize(ref reader, subType, clonedOptions);
        }

        throw new JsonException($"Discriminator {this.options.Discriminator} not found");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var clonedOptions = new JsonSerializerOptions(options);
        clonedOptions.Converters.Remove(this);
        JsonSerializer.Serialize(writer, value, value?.GetType() ?? typeof(T), clonedOptions);
    }
}
