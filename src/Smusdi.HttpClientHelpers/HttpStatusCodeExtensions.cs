using System.Net;
using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Smusdi.Extensibility;

namespace Smusdi.HttpClientHelpers;

public static class HttpStatusCodeExtensions
{
    public static bool IsValid(this HttpStatusCode code) => (int)code >= 200 && (int)code < 300;
}
