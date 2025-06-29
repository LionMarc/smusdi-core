# Testing services

## Dependencies

The tests and the testing helpers are using:

- [reqnroll](https://reqnroll.net/)
- [Awesome assertions](https://awesomeassertions.org/)


## Integration tests

For integration tests, the class **Smusdi.Testing.SmusdiTestingService** instanciates the **Smusdi.Core.SmusdiService** with **Microsoft.AspNetCore.TestHost.TestServer** instead of normal host.


The variable **ASPNETCORE_ENVIRONMENT** used to select the appsettings files to use is set to **reqnrol** before the initialization of the service.