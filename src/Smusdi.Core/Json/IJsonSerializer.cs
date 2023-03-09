using System.Text.Json;

namespace Smusdi.Core.Json;

public interface IJsonSerializer
{
    JsonSerializerOptions JsonSerializerOptions { get; }

    string Serialize<T>(T value);

    void Serialize<T>(T value, Stream stream);

    T? Deserialize<T>(string value);

    T? Deserialize<T>(Stream stream);
}
