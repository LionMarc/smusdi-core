using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core;

namespace Smusdi.Testing;

public sealed class CustomWebHostBuilder : IWebHostBuilder
{
    private readonly SmusdiService smusdiService;

    public CustomWebHostBuilder(SmusdiService smusdiService)
    {
        this.smusdiService = smusdiService;
    }

    public IWebHost Build()
    {
        this.smusdiService.Build();
        return new CustomWebHost(this.smusdiService);
    }

    public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        throw new NotImplementedException();
    }

    public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        this.smusdiService.WebApplicationBuilder?.WebHost.ConfigureServices(configureServices);
        return this;
    }

    public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
    {
        this.smusdiService.WebApplicationBuilder?.WebHost.ConfigureServices(configureServices);
        return this;
    }

    public string? GetSetting(string key)
    {
        throw new NotImplementedException();
    }

    public IWebHostBuilder UseSetting(string key, string? value)
    {
        throw new NotImplementedException();
    }
}
