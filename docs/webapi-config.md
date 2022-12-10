# Smusdi WEB API configuration

!!! note

    3 environment variables are used to setup the application configuration:

    - ASPNETCORE_ENVIRONMENT
    - SMUSDI_APPSETTINGS_FOLDER
    - SMUSDI_SERVICE_NAME

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
- Two appsettings files are added to the list of the configuration providers:

    - appsettings.%SMUSDI_SERVICE_NAME%.json
    - appsettings.%SMUSDI_SERVICE_NAME%_%ASPNETCORE_ENVIRONMENT%.json

The *appsetting.json* and *appsettings/%ASPNETCORE_ENVIRONMENT%.json* are shared by a list of services and define common settings. The two added ones contain only settings associated to the current service.

- The environment variables founded in the configuration properties are expanded by the provider added by the call to *EnableEnvironmentVariablesExpansion()*.

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
