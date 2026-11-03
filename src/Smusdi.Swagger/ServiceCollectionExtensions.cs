using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Smusdi.Extensibility;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Smusdi.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool?>("smusdi:NoVersioning") != true)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        services.AddSwaggerGen(options =>
        {
            if (configuration.GetValue<bool?>("smusdi:NoVersioning") == true)
            {
                var swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);
                var openApiInfo = new OpenApiInfo()
                {
                    Title = swaggerOptions.Title,
                    Description = swaggerOptions.Description,
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = swaggerOptions.ContactName,
                        Email = swaggerOptions.ContactMail,
                    },
                };
                options.SwaggerDoc("v1", openApiInfo);
            }

            ConfigureOAuthSecurity(options, configuration);

            foreach (var xmlFile in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
            {
                options.IncludeXmlComments(xmlFile);
            }
        });

        services.AddSingleton<ISwaggerUIOptionsConfigurator, SwaggerUIOptionsConfigurator>();

        return services;
    }

    private static void ConfigureOAuthSecurity(SwaggerGenOptions options, IConfiguration configuration)
    {
        var oauthSection = configuration.GetSection("oauth");
        var scopes = oauthSection.GetSection("scopes").Get<string[]>() ?? [];
        var mainAuthority = oauthSection.GetSection("mainAuthority");

        if (!string.IsNullOrWhiteSpace(mainAuthority["url"]))
        {
            AddOAuthSecurity(
                options,
                "oauth2",
                mainAuthority["url"]!,
                scopes,
                string.Equals(mainAuthority["type"], "Jwks", StringComparison.OrdinalIgnoreCase));
        }

        foreach (var authority in oauthSection.GetSection("additionalAuthorities").GetChildren())
        {
            var name = authority["name"];
            var url = authority["url"];
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(url))
            {
                AddOAuthSecurity(
                    options,
                    name,
                    url,
                    scopes,
                    string.Equals(authority["type"], "Jwks", StringComparison.OrdinalIgnoreCase));
            }
        }
    }

    private static void AddOAuthSecurity(SwaggerGenOptions options, string name, string url, IEnumerable<string> scopes, bool isJwks)
    {
        var authorityUrl = GetOAuthAuthorityUrl(url, isJwks);
        options.AddSecurityDefinition(name, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{authorityUrl}/protocol/openid-connect/auth"),
                    TokenUrl = new Uri($"{authorityUrl}/protocol/openid-connect/token"),
                    Scopes = new[] { "openid", "profile", "email" }
                        .Concat(scopes)
                        .ToDictionary(scope => scope, _ => string.Empty),
                },
            },
        });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(name, document)] = [],
        });
    }

    private static string GetOAuthAuthorityUrl(string url, bool isJwks)
    {
        const string jwksPath = "/protocol/openid-connect/certs";
        var authorityUrl = url.TrimEnd('/');
        return isJwks && authorityUrl.EndsWith(jwksPath, StringComparison.OrdinalIgnoreCase)
            ? authorityUrl[..^jwksPath.Length]
            : authorityUrl;
    }
}
