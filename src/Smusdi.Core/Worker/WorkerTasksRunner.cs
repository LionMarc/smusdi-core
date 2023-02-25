using ILogger = Serilog.ILogger;

namespace Smusdi.Core.Worker;

public sealed class WorkerTasksRunner : BackgroundService
{
    private readonly ILogger logger;
    private readonly IServiceProvider serviceProvider;
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    public WorkerTasksRunner(ILogger logger, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.Information("Executing registered tasks...");

        using (var scope = this.serviceProvider.CreateScope())
        {
            var workerTasks = scope.ServiceProvider.GetServices<IWorkerTask>();
            foreach (var workerTask in workerTasks)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    this.logger.Information($"Worker has been cancelled.");
                    return;
                }

                this.logger.Information($"Starting {workerTask.Name}.");

                await workerTask.Execute(stoppingToken);

                this.logger.Information($"{workerTask.Name} done.");
            }
        }

        this.logger.Information("All tasks done.");
        this.hostApplicationLifetime.StopApplication();
    }
}
