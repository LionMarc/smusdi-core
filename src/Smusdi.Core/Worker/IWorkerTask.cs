namespace Smusdi.Core.Worker;

public interface IWorkerTask
{
    string Name { get; }

    Task Execute(CancellationToken stoppingToken);
}
