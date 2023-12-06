using System.Text.Json;
using System.Text;
using Smusdi.Core.Worker;

namespace Smusdi.Worker.Sample;

internal sealed class ShortRunningWorkerTask : IWorkerTask
{
    public string Name => "Short running worker task";

    public Task Execute(CancellationToken stoppingToken) => Task.Delay(1000, stoppingToken);
}

internal sealed class JsonTask : IWorkerTask
{
    public string Name => "Json worker task";

    public async Task Execute(CancellationToken stoppingToken)
    {
        TestJson();
    }

    private static void TestJson()
    {
        var jsonString = @"{
                ""Date"": ""2019-08-01T00:00:00-07:00"",
                ""Temperature"": 25,
                ""TemperatureRanges"": {
                    ""Cold"": { ""High"": 20, ""Low"": -10 },
                    ""Hot"": { ""High"": 60, ""Low"": 20 }
                },
                ""Summary"": ""Hot"",
            }";

        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
        var stream = new MemoryStream(bytes);

        var buffer = new byte[10];

        // Fill the buffer.
        // For this snippet, we're assuming the stream is open and has data.
        // If it might be closed or empty, check if the return value is 0.
        stream.Read(buffer);

        // We set isFinalBlock to false since we expect more data in a subsequent read from the stream.
        var reader = new Utf8JsonReader(buffer, isFinalBlock: false, state: default);
        Console.WriteLine($"String in buffer is: {Encoding.UTF8.GetString(buffer)}");

        // Search for "Summary" property name
        while (reader.TokenType != JsonTokenType.PropertyName || !reader.ValueTextEquals("Summary"))
        {
            if (!reader.Read())
            {
                // Not enough of the JSON is in the buffer to complete a read.
                GetMoreBytesFromStream(stream, ref buffer, ref reader);
            }
        }

        // Found the "Summary" property name.
        Console.WriteLine($"String in buffer is: {Encoding.UTF8.GetString(buffer)}");
        while (!reader.Read())
        {
            // Not enough of the JSON is in the buffer to complete a read.
            GetMoreBytesFromStream(stream, ref buffer, ref reader);
        }
        // Display value of Summary property, that is, "Hot".
        Console.WriteLine($"Got property value: {reader.GetString()}");
    }

    private static void GetMoreBytesFromStream(
            MemoryStream stream, ref byte[] buffer, ref Utf8JsonReader reader)
    {
        int bytesRead;
        if (reader.BytesConsumed < buffer.Length)
        {
            ReadOnlySpan<byte> leftover = buffer.AsSpan((int)reader.BytesConsumed);

            if (leftover.Length == buffer.Length)
            {
                Array.Resize(ref buffer, buffer.Length * 2);
                Console.WriteLine($"Increased buffer size to {buffer.Length}");
            }

            leftover.CopyTo(buffer);
            bytesRead = stream.Read(buffer.AsSpan(leftover.Length));
        }
        else
        {
            bytesRead = stream.Read(buffer);
        }
        Console.WriteLine($"String in buffer is: {Encoding.UTF8.GetString(buffer)}");
        reader = new Utf8JsonReader(buffer, isFinalBlock: bytesRead == 0, reader.CurrentState);
    }
}
