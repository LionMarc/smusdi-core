using Smusdi.Core.Worker;

namespace Smusdi.Worker.Sample;

internal sealed class ShortRunningWorkerTask : IWorkerTask
{
    public string Name => "Short running worker task";

    public int Order => 10;

    public Task Execute(CancellationToken stoppingToken) => Task.Delay(1000, stoppingToken);
}
