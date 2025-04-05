using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace Smusdi.Core.Worker;

public sealed class WorkerTasksRunner(ILogger logger, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.Information("Executing registered tasks...");

        using (var scope = serviceProvider.CreateScope())
        {
            var workerTasks = scope.ServiceProvider.GetServices<IWorkerTask>();
            foreach (var workerTask in workerTasks.OrderBy(t => t.Order))
            {
                using (LogContext.PushProperty("WorkerTaskName", workerTask.Name))
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        logger.Information("Task has been cancelled.");
                        return;
                    }

                    logger.Information("Starting task");

                    await workerTask.Execute(stoppingToken);

                    logger.Information("Task done.");
                }
            }
        }

        logger.Information("All tasks done.");
        hostApplicationLifetime.StopApplication();
    }
}
