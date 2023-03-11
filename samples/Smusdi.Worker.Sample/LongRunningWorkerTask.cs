using Smusdi.Core.Worker;

namespace Smusdi.Worker.Sample;

internal sealed class LongRunningWorkerTask : IWorkerTask
{
    private readonly ISampleService sampleService;

    public LongRunningWorkerTask(ISampleService sampleService)
    {
        this.sampleService = sampleService;
    }

    public string Name => "Long running worker task";

    public async Task Execute(CancellationToken stoppingToken)
    {
        await this.sampleService.Test("scope1");
        await this.sampleService.Test("scope2");
        await Task.Delay(10000, stoppingToken);
    }
}
