using Smusdi.Core.Worker;

namespace Smusdi.Worker.Sample;

internal sealed class LongRunningWorkerTask : IWorkerTask
{
    public string Name => "Long running worker task";

    public Task Execute(CancellationToken stoppingToken) => Task.Delay(10000, stoppingToken);
}
