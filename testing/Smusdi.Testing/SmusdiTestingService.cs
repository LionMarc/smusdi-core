using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core;
using Smusdi.Core.Extensibility;
using Smusdi.Testing.Http;

namespace Smusdi.Testing;

public sealed class SmusdiTestingService : IAsyncDisposable, IDisposable
{
    // 0 = not started, 1 = started
    private int started;

    // 0 = not disposed, 1 = disposed
    private int disposed;

    public SmusdiTestingService()
    {
        this.Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public SmusdiService? SmusdiService { get; private set; }

    public TestServer? TestServer { get; private set; }

    public HttpClient? TestClient { get; private set; }

    public void Initialize(string[] args)
    {
        this.SmusdiService = new SmusdiService();
        this.SmusdiService.CreateAndInitializeBuider(args);

        var builder = this.SmusdiService.WebApplicationBuilder;
        if (builder != null)
        {
            builder.Services.AddApplicationServices<ITestingServicesRegistrator>(builder.Configuration);

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Test";
                o.DefaultChallengeScheme = "Test";
            })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

            builder.WebHost.UseTestServer();

            builder.Services.AddSingleton<ClaimsProvider>();
        }

        this.SmusdiService.Build();

        this.SmusdiService.ConfigureWebApplication();

        this.TestServer = this.SmusdiService.WebApplication?.GetTestServer();
    }

    public async Task StartAsync()
    {
        // Ensure StartAsync is executed only once
        if (Interlocked.Exchange(ref this.started, 1) == 1)
        {
            throw new InvalidOperationException("StartAsync can only be called once on SmusdiTestingService.");
        }

        if (this.SmusdiService != null && this.SmusdiService.WebApplication != null)
        {
            await this.SmusdiService.ExecuteBeforeRunImplementations();
            await this.SmusdiService.WebApplication.StartAsync();
        }

        // Dispose any existing client (defensive)
        this.TestClient?.Dispose();
        this.TestClient = this.TestServer?.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        // Ensure dispose is executed only once
        if (Interlocked.Exchange(ref this.disposed, 1) == 1)
        {
            return;
        }

        // Dispose TestClient first as it may hold references to server resources
        this.TestClient?.Dispose();
        this.TestClient = null;

        // Stop the web application if it was started
        if (this.SmusdiService != null && this.SmusdiService.WebApplication != null)
        {
            try
            {
                await this.SmusdiService.WebApplication.StopAsync();
            }
            catch
            {
                // Swallow exceptions on stop to ensure best-effort cleanup
            }
        }

        // Dispose TestServer
        this.TestServer?.Dispose();
        this.TestServer = null;

        // Dispose SmusdiService (synchronous). It will dispose the WebApplication as needed.
        this.SmusdiService?.Dispose();
        this.SmusdiService = null;
    }

    public void Dispose()
    {
        // Call async dispose synchronously
        this.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    public T? GetService<T>()
    {
        var serviceProvider = this.SmusdiService?.WebApplication?.Services;
        if (serviceProvider == null)
        {
            return default;
        }

        return serviceProvider.GetService<T>();
    }

    public T GetRequiredService<T>()
        where T : notnull
    {
        var serviceProvider = this.SmusdiService?.WebApplication?.Services;
        if (serviceProvider == null)
        {
            throw new InvalidOperationException("SmusdiService is not initialized");
        }

        return serviceProvider.GetRequiredService<T>();
    }

    public IEnumerable<T> GetServices<T>()
    {
        var serviceProvider = this.SmusdiService?.WebApplication?.Services;
        if (serviceProvider == null)
        {
            return [];
        }

        return serviceProvider.GetServices<T>();
    }
}
