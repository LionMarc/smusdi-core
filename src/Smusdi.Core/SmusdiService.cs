using System.IO.Abstractions;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;
using Smusdi.Core.Configuration;
using Smusdi.Core.Extensibility;
using Smusdi.Core.HealthChecks;
using Smusdi.Core.Info;
using Smusdi.Core.Logging;
using Smusdi.Core.Oauth;
using Smusdi.Core.Swagger;

namespace Smusdi.Core;

public class SmusdiService : IDisposable
{
    private const string CorsPolicy = "MyCorsPolicy";

    public WebApplicationBuilder? WebApplicationBuilder { get; private set; }

    public WebApplication? WebApplication { get; private set; }

    public static void InitAndRun(string[] args)
    {
        var smusdiService = new SmusdiService();
        smusdiService.CreateAndInitializeBuider(args);
        smusdiService.Build();
        smusdiService.ConfigureWebApplication();

        smusdiService.Run();
    }

    public void CreateAndInitializeBuider(string[] args)
    {
        var webApplicationOptions = new WebApplicationOptions
        {
            Args = args,
        };

        var builder = WebApplication.CreateBuilder(webApplicationOptions)
            .InitConfiguration()
            .InitLoggerConfiguration();

        builder.Services
            .AddSingleton<IFileSystem, FileSystem>()
            .AddAllHealthChecks()
            .AddEndpointsApiExplorer()
            .AddSwagger(builder.Configuration)
            .AddSecurity(builder.Configuration)
            .AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

        var mvcBuilder = builder.Services.AddControllers().AddJsonOptions(j => j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        mvcBuilder.AddParts();

        builder.Services
            .AddCors(o => o.AddPolicy(CorsPolicy, builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblies(ScrutorHelpers.GetAllReferencedAssembliesWithTypeAssignableTo<IValidator>());

        builder.Services.AddApplicationServices<IServicesRegistrator>(builder.Configuration);

        this.WebApplicationBuilder = builder;
    }

    public void Build()
    {
        if (this.WebApplicationBuilder == null)
        {
            return;
        }

        this.WebApplication = this.WebApplicationBuilder.Build();
    }

    public void ConfigureWebApplication()
    {
        if (this.WebApplication == null)
        {
            return;
        }

        this.WebApplication.UseCors(CorsPolicy);
        this.WebApplication.UseHttpLogging();
        this.WebApplication.UseSwagger();
        this.WebApplication.UseSecurity(this.WebApplication.Configuration);
        this.WebApplication.MapControllers();
        this.WebApplication
            .UseSecuredSwaggerUI(this.WebApplication.Configuration)
            .UseHealthChecks()
            .UseInfoEndpoint();
    }

    public Task RunAsync() => this.WebApplication?.RunAsync() ?? Task.CompletedTask;

    public void Run() => this.WebApplication?.Run();

    public void Dispose()
    {
        this.WebApplication?.DisposeAsync().GetAwaiter().GetResult();
        this.WebApplication = null;
        this.WebApplicationBuilder = null;
    }
}
