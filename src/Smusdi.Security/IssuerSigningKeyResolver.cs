using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Serilog;
using Smusdi.Extensibility;

namespace Smusdi.Security;

public sealed class IssuerSigningKeyResolver(IMemoryCache memoryCache, IServiceProvider serviceProvider, ILogger logger) : IIssuerSigningKeyResolver
{
    private readonly Lock @lock = new();

    public IEnumerable<SecurityKey> GetIssuerSigningKeys(string jwksUrl, string kid, TimeSpan cacheDuration)
    {
        var cacheKey = $"{jwksUrl}_{kid}";
        if (memoryCache.TryGetValue<IEnumerable<SecurityKey>>(cacheKey, out var securityKeys) && securityKeys != null)
        {
            return securityKeys;
        }

        lock (this.@lock)
        {
            if (memoryCache.TryGetValue(cacheKey, out securityKeys) && securityKeys != null)
            {
                return securityKeys;
            }

            var message = $"[IssuerSigningKeyResolver] Loading signing keys from {jwksUrl} with cache duration {cacheDuration} for kid {kid}";
            logger.Information(message);
            var signingKeysLoader = serviceProvider.GetRequiredService<ISigningKeysLoader>();
            securityKeys = signingKeysLoader.LoadSigningKeys(jwksUrl);
            memoryCache.Set(cacheKey, securityKeys, cacheDuration);
            return securityKeys;
        }
    }
}
