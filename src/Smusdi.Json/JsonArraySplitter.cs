using System.Buffers;
using System.Text;
using System.Text.Json;

namespace Smusdi.Json;

/// <summary>
/// Helper class used to split a root JSON array and return each json item in an output stream.
/// There is no deserialization.The class takes as input a json string and returns a list of json strings.
/// Everything is done with <see cref="System.Text.Json.Utf8JsonReader"/> and <see cref="System.Text.Json.Utf8JsonWriter"/>.
/// </summary>
public sealed class JsonArraySplitter
{
    private readonly Stream inputStream;
    private readonly int generatedJsonBufferSize;
    private byte[] buffer = new byte[1024];
    private JsonReaderState jsonReaderState = default;
    private bool isFinalBlock = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArraySplitter"/> class.
    /// </summary>
    /// <param name="stream">The stream providing the input json array.</param>
    /// <param name="generatedJsonBufferSize">The size of the in-memory buffer for the generated output.</param>
    /// <exception cref="JsonArraySplitterException">In case input file is empty.</exception>
    public JsonArraySplitter(Stream stream, int generatedJsonBufferSize = 1024)
    {
        this.inputStream = stream;
        this.generatedJsonBufferSize = generatedJsonBufferSize;
        this.InitializeBuffer();
        this.ReadStartArrayToken();
    }

    public bool ReadNextItem(Stream outputStream)
    {
        var utf8JsonReader = this.NewUtf8JsonReader();

        // First search for start of object
        List<JsonTokenType> notExpectedJsonTypes = [JsonTokenType.True, JsonTokenType.False, JsonTokenType.String, JsonTokenType.Null, JsonTokenType.Number];
        while (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.EndArray)
        {
            this.ReadNextToken(ref utf8JsonReader);

            if (notExpectedJsonTypes.Contains(utf8JsonReader.TokenType))
            {
                throw new JsonArraySplitterException("Class must be used to split array of objects only.");
            }
        }

        if (utf8JsonReader.TokenType == JsonTokenType.EndArray)
        {
            return false;
        }

        // Maybe, store the writer as class instance inorder to reuse it. Set this class as IDisposable and dispose the writer.
        using var utf8JsonWriter = new Utf8JsonWriter(outputStream);
        utf8JsonWriter.WriteStartObject();

        while (utf8JsonReader.TokenType != JsonTokenType.EndObject || utf8JsonReader.CurrentDepth > 1)
        {
            var jsonToken = this.ReadNextToken(ref utf8JsonReader);
            this.WriteReadJsonToken(ref utf8JsonReader, jsonToken, utf8JsonWriter);
        }

        utf8JsonWriter.Flush();

        this.CacheStateForLaterUse(ref utf8JsonReader);

        return true;
    }

    private void InitializeBuffer()
    {
        var bytesCount = this.inputStream.Read(this.buffer);
        var utf8Bom = Encoding.UTF8.GetPreamble();
        var leftOver = this.buffer.AsSpan();
        if (leftOver.StartsWith(utf8Bom))
        {
            leftOver = leftOver.Slice(utf8Bom.Length);
            leftOver.CopyTo(this.buffer);
            Array.Clear(this.buffer, leftOver.Length, this.buffer.Length - leftOver.Length);
            var extraBytesCount = this.inputStream.Read(this.buffer.AsSpan(leftOver.Length));
            bytesCount -= utf8Bom.Length;
            bytesCount += extraBytesCount;
        }

        if (bytesCount == 0)
        {
            throw new JsonArraySplitterException("Nothing to read from input stream.");
        }
    }

    private Utf8JsonReader NewUtf8JsonReader() => new(this.buffer, this.isFinalBlock, this.jsonReaderState);

    private void ReadStartArrayToken()
    {
        var utf8JsonReader = this.NewUtf8JsonReader();
        while (utf8JsonReader.TokenType != JsonTokenType.StartArray)
        {
            this.ReadNextToken(ref utf8JsonReader);
            if (utf8JsonReader.TokenType == JsonTokenType.StartObject)
            {
                throw new JsonArraySplitterException("Expected a root array, not an object.");
            }
        }

        this.CacheStateForLaterUse(ref utf8JsonReader);
    }

    private string ReadNextToken(ref Utf8JsonReader utf8JsonReader)
    {
        var positionBefore = utf8JsonReader.BytesConsumed;

        while (!utf8JsonReader.Read() && !this.isFinalBlock)
        {
            this.GetMoreBytesFromStream(ref utf8JsonReader);
            positionBefore = 0;
        }

        var positionAfter = utf8JsonReader.BytesConsumed;
        if (positionAfter < positionBefore)
        {
            throw new JsonArraySplitterException($"Unexpected situation: positionBefore={positionBefore}, positionAfter={positionAfter}");
        }

        switch (utf8JsonReader.TokenType)
        {
            case JsonTokenType.StartArray:
                return "[";

            case JsonTokenType.EndArray:
                return "]";

            case JsonTokenType.StartObject:
                return "{";

            case JsonTokenType.EndObject:
                return "}";

            default:
                {
                    ReadOnlySpan<byte> jsonElement = utf8JsonReader.HasValueSequence ?
                       utf8JsonReader.ValueSequence.ToArray() :
                       utf8JsonReader.ValueSpan;
                    return Encoding.UTF8.GetString(jsonElement);
                }
        }
    }

    private void CacheStateForLaterUse(ref Utf8JsonReader utf8JsonReader)
    {
        if (utf8JsonReader.BytesConsumed > 0)
        {
            ReadOnlySpan<byte> leftover = this.buffer.AsSpan((int)utf8JsonReader.BytesConsumed);
            leftover.CopyTo(this.buffer);
            _ = this.inputStream.Read(this.buffer.AsSpan(leftover.Length));
        }

        this.jsonReaderState = utf8JsonReader.CurrentState;
    }

    private void GetMoreBytesFromStream(ref Utf8JsonReader reader)
    {
        int bytesRead;
        if (reader.BytesConsumed < this.buffer.Length)
        {
            ReadOnlySpan<byte> leftover = this.buffer.AsSpan((int)reader.BytesConsumed);

            if (leftover.Length == this.buffer.Length)
            {
                Array.Resize(ref this.buffer, this.buffer.Length * 2);
            }

            leftover.CopyTo(this.buffer);
            Array.Clear(this.buffer, leftover.Length, this.buffer.Length - leftover.Length);
            bytesRead = this.inputStream.Read(this.buffer.AsSpan(leftover.Length));
        }
        else
        {
            bytesRead = this.inputStream.Read(this.buffer);
        }

        this.isFinalBlock = bytesRead == 0;
        this.jsonReaderState = reader.CurrentState;
        reader = this.NewUtf8JsonReader();
    }

    private void WriteReadJsonToken(ref Utf8JsonReader utf8JsonReader, string jsonToken, Utf8JsonWriter utf8JsonWriter)
    {
        switch (utf8JsonReader.TokenType)
        {
            case JsonTokenType.StartObject:
                utf8JsonWriter.WriteStartObject();
                break;

            case JsonTokenType.EndObject:
                utf8JsonWriter.WriteEndObject();
                break;

            case JsonTokenType.StartArray:
                utf8JsonWriter.WriteStartArray();
                break;

            case JsonTokenType.EndArray:
                utf8JsonWriter.WriteEndArray();
                break;

            case JsonTokenType.PropertyName:
                utf8JsonWriter.WritePropertyName(jsonToken);
                break;

            case JsonTokenType.String:
                utf8JsonWriter.WriteRawValue($"\"{jsonToken}\"");
                break;

            case JsonTokenType.Number:
            case JsonTokenType.Null:
            case JsonTokenType.True:
            case JsonTokenType.False:
                utf8JsonWriter.WriteRawValue(jsonToken);
                break;
        }

        this.FlushIfRequired(utf8JsonWriter);
    }

    private void FlushIfRequired(Utf8JsonWriter utf8JsonWriter)
    {
        if (utf8JsonWriter.BytesPending >= this.generatedJsonBufferSize)
        {
            utf8JsonWriter.Flush();
        }
    }
}
