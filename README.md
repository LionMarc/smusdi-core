# smusdi-core
![build workflow](https://github.com/LionMarc/smusdi-core/actions/workflows/build.yml/badge.svg)
[![license](https://img.shields.io/badge/License-MIT-purple.svg)](LICENSE)
[![HitCount](https://hits.dwyl.com/LionMarc/smusdi-core.svg?style=flat-square)](http://hits.dwyl.com/LionMarc/smusdi-core)
[![NuGet stable version](https://badgen.net/nuget/v/Smusdi.Core)](https://nuget.org/packages/Smusdi.Core)

This project provides utility libraries for .NET services.

## Smusdi.Core

Bootstrapper for .NET services.

Just add the package as reference and set the following code in *program.cs* file of your api project:

```cs
using Smusdi.Core;

SmusdiService.InitAndRun(args);
```

The bootstrapper uses 2 environment variables:

- **SMUSDI_SERVICE_NAME**: the name of the service which will be used for the appsettings files, the logs files...
- **SMUSDI_APPSETTINGS_FOLDER**: this variable setup the folder in which the service can find the appsettings files. If not set, the appsettings files are supposed to be in the working directory.

> The bootstrapper load the following appsettings files:
> - appsettings.json
> - appsettings.{ASPNETCORE_ENVIRONMENT}.json
> - appsettings.{SMUSDI_SERVICE_NAME}.json
> - appsettings.{SMUSDI_SERVICE_NAME}.{ASPNETCORE_ENVIRONMENT}.json

- **SMUSDI_SERVICE_VERSION**: used by the *info* endpoint to send the service version;
- **SMUSDI_CUSTOM_INFOS_FOLDER** used by the *info* endpoint to send custom informations.

A default *appsettings.json* file can be found in the *samples* folder.

## Extension Points

The project provides two extension points:

- **IServicesRegistrator** used to register custom services
  ```C#
  public interface IServicesRegistrator : IBaseServicesRegistrator
  {
  }
  
  // Used by Smusdi.Testing to allow overriding some services when testing application
  public interface ITestingServicesRegistrator : IBaseServicesRegistrator
  {
  }
  
  public interface IBaseServicesRegistrator
  {
      IServiceCollection Add(IServiceCollection services, IConfiguration configuration);
  }
  ```
- **IWebApplicationConfigurator** to add some configuration actions to the web application
  ```C#
  public interface IWebApplicationConfigurator
  {
      WebApplication Configure(WebApplication webApplication);
  }
  ```
   
> The implementations of these interfaces are automatically executed. There are discovered with [scrutor](https://github.com/khellang/Scrutor).

## Tests and coverage

```
dotnet test --collect:"XPlat Code Coverage" -r results
dotnet tool run reportgenerator -reports:"results/**/*.xml" -targetdir:coveragereport -reporttypes:html
```

> *reportgenerator* is installed as a local tool

## Publishing as single-file

As reflection is limited when publishing application as a single-file, you have to setup in the appsettings file the list of assemblies in which the controlles or the extensions classes can be found.

See the sample *SingleFilePublication.Service*.

```json
{
    "smusdi": {
        "assemblyNames": [
          "SingleFilePublication",
          "SingleFilePublication.SomeFeature"
        ]
      }
}
```

> If an assembly can not be found, it is ignored.

