using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Smusdi.Extensibility;

namespace Smusdi.Security;

internal sealed class ServicesRegistrator : IServicesRegistrator
{
    public IServiceCollection Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IIssuerSigningKeyResolver, IssuerSigningKeyResolver>()
            .AddHttpClient<ISigningKeysLoader, SigningKeysLoader>();

        return services;
    }
}
