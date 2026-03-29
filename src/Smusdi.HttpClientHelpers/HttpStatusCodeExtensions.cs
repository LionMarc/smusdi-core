using System.Net;

namespace Smusdi.HttpClientHelpers;

public static class HttpStatusCodeExtensions
{
    public static bool IsValid(this HttpStatusCode code) => (int)code >= 200 && (int)code < 300;
}
