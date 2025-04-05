using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Smusdi.Extensibility;

namespace Smusdi.Security;

public sealed class SigningKeysLoader(HttpClient httpClient) : ISigningKeysLoader
{
    public IEnumerable<SecurityKey> LoadSigningKeys(string jwksUrl)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, jwksUrl);
        var response = httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStringAsync().Result;
        var keys = new JsonWebKeySet(content);
        return keys.GetSigningKeys();
    }
}
