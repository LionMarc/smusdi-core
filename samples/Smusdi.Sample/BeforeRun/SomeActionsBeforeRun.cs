using Serilog;
using Smusdi.Core.Extensibility;

namespace Smusdi.Sample.BeforeRun;

internal class SomeActionsBeforeRun : IBeforeRun
{
    private readonly ITestingService testingService;
    private readonly ILogger logger;

    public SomeActionsBeforeRun(ITestingService testingService, ILogger logger)
    {
        this.testingService = testingService;
        this.logger = logger;
    }

    public async Task Execute()
    {
        this.logger.Information("Executing SomeActionsBeforeRun...");
        await this.testingService.Run();
        this.logger.Information("SomeActionsBeforeRun executed.");
    }
}
