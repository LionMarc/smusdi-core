using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Smusdi.Core.Extensibility;

namespace Smusdi.Sample.BeforeRun;

internal class TestingService : ITestingService
{
    private readonly ILogger logger;

    public TestingService(ILogger logger) => this.logger = logger;

    public async Task Run()
    {
        this.logger.Information("TestingService called...");

        await Task.Delay(10000);

        this.logger.Information("TestingService executed");
    }
}
