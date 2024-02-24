using Microsoft.Extensions.Hosting;
using Serilog;
using Smusdi.Core.Worker;

namespace Smusdi.Sample.BackgroundServices;

internal sealed class CoucoucDeferredBackgroundService(IHostApplicationLifetime hostApplicationLifetime, ILogger logger) : DeferredBackgroundService(hostApplicationLifetime)
{
    private readonly ILogger logger = logger;

    protected override async Task OnHostStarted(CancellationToken stoppingToken)
    {
        this.logger.Information("From Coucou background service.");
        await Task.Delay(30000);
        this.logger.Information("Coucou background service is over.");
    }
}
