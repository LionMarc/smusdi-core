namespace Smusdi.Core.Worker;

public abstract class DeferredBackgroundService(IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly IHostApplicationLifetime hostApplicationLifetime = hostApplicationLifetime;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.hostApplicationLifetime.ApplicationStarted.Register(() => this.OnHostStarted(stoppingToken));
        return Task.CompletedTask;
    }

    protected abstract Task OnHostStarted(CancellationToken stoppingToken);
}
