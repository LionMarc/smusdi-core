namespace Smusdi.Core.Worker;

public interface IWorkerTask
{
    string Name { get; }

    int Order { get; }

    Task Execute(CancellationToken stoppingToken);
}
