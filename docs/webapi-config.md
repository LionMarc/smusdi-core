# Smusdi WEB API configuration

!!! note

    3 environment variables are used to setup the application configuration:

    - ASPNETCORE_ENVIRONMENT
    - SMUSDI_APPSETTINGS_FOLDER
    - SMUSDI_SERVICE_NAME
    - SMUSDI_APPSETTINGS_RELOAD_ON_CHANGE

## Implementation overview

The Smusdi service adds some configuration providers to the default ones and update the default json files configuration providers.

```cs
var webApplicationOptions = new WebApplicationOptions
        {
            Args = args,
        };

var builder = WebApplication.CreateBuilder(webApplicationOptions)
    .InitConfiguration();
```

And the *InitConfiguration* extension method:
```cs
{% include "../src/Smusdi.Core/Configuration/ConfigurationBuilding.cs" %}
```

So, currently:

- The base path of the json appsettings file is set to the value of the **SMUSDI_APPSETTINGS_FOLDER** environment variable if the target folder exists;
- Appsettings files are watched for changes by default; set **SMUSDI_APPSETTINGS_RELOAD_ON_CHANGE** to `false` to disable watching;
- Two appsettings files are added to the list of the configuration providers:

    - appsettings.%SMUSDI_SERVICE_NAME%.json
    - appsettings.%SMUSDI_SERVICE_NAME%_%ASPNETCORE_ENVIRONMENT%.json

The *appsetting.json* and *appsettings/%ASPNETCORE_ENVIRONMENT%.json* are shared by a list of services and define common settings. The two added ones contain only settings associated to the current service.

- The environment variables founded in the configuration properties are expanded by the provider added by the call to *EnableEnvironmentVariablesExpansion()*.

## Reverse proxy path prefixes

When an ingress or reverse proxy publishes the service under a URL prefix, configure the public URL in the `smusdi` section:

```json
{
  "smusdi": {
    "reverseProxy": {
      "server": "https://api.example.com",
      "basePath": "/my-service"
    }
  }
}
```

`server` is optional and must be an absolute HTTP or HTTPS URL without a query or fragment. `basePath` is optional and is the public path prefix. Smusdi adds the prefix to the request path base and to the native OpenAPI document's `servers` URL. The middleware supports ingress configurations that either preserve the prefix in the forwarded request path or strip it before forwarding.

To use `X-Forwarded-For`, `X-Forwarded-Proto`, and `X-Forwarded-Host`, set `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true`. When enabled, Smusdi accepts these headers from any source; only enable this when the service is reachable exclusively through a trusted proxy that overwrites forwarded headers.

## Specs

```gherkin
{% include "../specs/Smusdi.Core.Specs/Configuration/AppsettingsFolderSelection.feature" %}
```

```gherkin
{% include "../specs/Smusdi.Core.Specs/Configuration/SmusdiServiceSpecificAppSettings.feature" %}
```

```gherkin
{% include "../specs/Smusdi.Core.Specs/Configuration/EnvironmentVariablesExpansion.feature" %}
```
