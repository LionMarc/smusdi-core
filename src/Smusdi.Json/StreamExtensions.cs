namespace Smusdi.Json;

public static class StreamExtensions
{
    public static int ReadBytes(this Stream stream, byte[] buffer, int startIndex, int count)
    {
        var totalBytesRead = 0;
        while (totalBytesRead < count)
        {
            var bytesRead = stream.Read(buffer, startIndex + totalBytesRead, count - totalBytesRead);
            totalBytesRead += bytesRead;
            if (bytesRead == 0)
            {
                break;
            }
        }

        return totalBytesRead;
    }
}
