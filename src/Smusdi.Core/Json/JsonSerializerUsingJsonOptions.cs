using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Smusdi.Core.Json;

public class JsonSerializerUsingJsonOptions : IJsonSerializer
{
    public JsonSerializerUsingJsonOptions(IOptions<JsonOptions> jsonOptions) => this.JsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;

    public JsonSerializerOptions JsonSerializerOptions { get; }

    public T? Deserialize<T>(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(value, this.JsonSerializerOptions);
    }

    public ValueTask<T?> DeserializeAsync<T>(Stream stream)
    {
        return JsonSerializer.DeserializeAsync<T>(stream, this.JsonSerializerOptions);
    }

    public T? Deserialize<T>(Stream stream)
    {
        return JsonSerializer.Deserialize<T>(stream, this.JsonSerializerOptions);
    }

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, this.JsonSerializerOptions);

    public void Serialize<T>(T value, Stream stream) => JsonSerializer.Serialize(stream, value, this.JsonSerializerOptions);

    public Task SerializeAsync<T>(T value, Stream stream) => JsonSerializer.SerializeAsync(stream, value, this.JsonSerializerOptions);
}
