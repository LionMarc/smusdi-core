# API versioning

The versioning of the API is managed with the package [aspnet-api-versioning](https://github.com/dotnet/aspnet-api-versioning).

!!! note

    If the service does not use api versioning, the configuration parameter **noVersioning** in appsettings file must be set to false.

    ```json
    {
        "smusdi":{
            "noVersioning": false
        }
    }
    ```

A versioned controller must be tagged as follows:

```cs
[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/sample")]
public class SampleController : ControllerBase
{
    ...
}
```
