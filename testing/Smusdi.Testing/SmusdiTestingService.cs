using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core;
using Smusdi.Core.Extensibility;

namespace Smusdi.Testing;

public sealed class SmusdiTestingService : IDisposable
{
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

        this.TestServer = new TestServer(new CustomWebHostBuilder(this.SmusdiService));

        this.SmusdiService.ConfigureWebApplication();
    }

    public async Task StartAsync()
    {
        if (this.SmusdiService != null && this.SmusdiService.WebApplication != null)
        {
            await this.SmusdiService.ExecuteBeforeRunImplementations();
            await this.SmusdiService.WebApplication.StartAsync();
        }

        this.TestClient = this.TestServer?.CreateClient();
    }

    public void Dispose()
    {
        this.SmusdiService?.Dispose();
        this.SmusdiService = null;
        this.TestServer?.Dispose();
        this.TestServer = null;
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
