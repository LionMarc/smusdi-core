using Serilog.Context;
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
            foreach (var workerTask in workerTasks.OrderBy(t => t.Order))
            {
                using (LogContext.PushProperty("WorkerTaskName", workerTask.Name))
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        this.logger.Information("Task has been cancelled.");
                        return;
                    }

                    this.logger.Information("Starting task");

                    await workerTask.Execute(stoppingToken);

                    this.logger.Information("Task done.");
                }
            }
        }

        this.logger.Information("All tasks done.");
        this.hostApplicationLifetime.StopApplication();
    }
}
