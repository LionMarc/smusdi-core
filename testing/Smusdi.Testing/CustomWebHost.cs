using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Smusdi.Core;

namespace Smusdi.Testing;

public sealed class CustomWebHost : IWebHost
{
    private readonly SmusdiService smusdiService;

    public CustomWebHost(SmusdiService smusdiService)
    {
        this.smusdiService = smusdiService;
    }

    public IFeatureCollection ServerFeatures => throw new NotImplementedException();

    public IServiceProvider Services => this.smusdiService.WebApplication?.Services ?? throw new InvalidOperationException();

    public void Dispose()
    {
        // nothing here
    }

    public void Start()
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
