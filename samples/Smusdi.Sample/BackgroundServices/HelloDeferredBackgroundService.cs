using Microsoft.Extensions.Hosting;
using Serilog;
using Smusdi.Core.Worker;

namespace Smusdi.Sample.BackgroundServices;

internal sealed class HelloDeferredBackgroundService(IHostApplicationLifetime hostApplicationLifetime, ILogger logger) : DeferredBackgroundService(hostApplicationLifetime)
{
    private readonly ILogger logger = logger;

    protected override async Task OnHostStarted(CancellationToken stoppingToken)
    {
        this.logger.Information("From Hello background service.");
        await Task.Delay(30000);
        this.logger.Information("Hello background service is over.");
    }
}
