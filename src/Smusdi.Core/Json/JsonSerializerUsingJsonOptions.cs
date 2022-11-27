using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Smusdi.Core.Json;

public class JsonSerializerUsingJsonOptions : IJsonSerializer
{
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public JsonSerializerUsingJsonOptions(IOptions<JsonOptions> jsonOptions)
    {
        this.jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
    }

    public T? Deserialize<T>(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        return System.Text.Json.JsonSerializer.Deserialize<T>(value, this.jsonSerializerOptions);
    }

    public string Serialize<T>(T value) => System.Text.Json.JsonSerializer.Serialize(value, this.jsonSerializerOptions);
}
