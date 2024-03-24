using System.IO.Abstractions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.FileProviders;
using Smusdi.Core.Compression;
using Smusdi.Core.Configuration;
using Smusdi.Core.Extensibility;
using Smusdi.Core.HealthChecks;
using Smusdi.Core.Helpers;
using Smusdi.Core.Info;
using Smusdi.Core.Json;
using Smusdi.Core.Logging;
using Smusdi.Core.Multipart;
using Smusdi.Core.Oauth;
using Smusdi.Core.Pipeline;
using Smusdi.Core.Swagger;
using Smusdi.Core.Validation;

namespace Smusdi.Core;

public class SmusdiService : IDisposable
{
    public const string CorsPolicy = "MyCorsPolicy";

    public WebApplicationBuilder? WebApplicationBuilder { get; private set; }

    public WebApplication? WebApplication { get; private set; }

    public static Task InitAndRunAsync(string[] args) => InitAndRunAsync<SmusdiService>(args);

    public static async Task InitAndRunAsync<T>(string[] args)
        where T : SmusdiService, new()
    {
        var service = new T();
        service.CreateAndInitializeBuider(args);
        service.Build();
        service.ConfigureWebApplication();

        await service.ExecuteBeforeRunImplementations();

        service.Run();
    }

    public static void ReadEnvFileIfExists()
    {
        var envFile = Environment.GetEnvironmentVariable(SmusdiConstants.SmusdiEnvFilePath) ?? SmusdiConstants.DefaultEnvFile;
        if (!string.IsNullOrWhiteSpace(envFile) && File.Exists(envFile))
        {
            foreach (var line in File.ReadAllLines(envFile))
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    var value = Environment.ExpandEnvironmentVariables(parts[1]);
                    Environment.SetEnvironmentVariable(parts[0], value);
                }
            }
        }
    }

    public virtual void CreateAndInitializeBuider(string[] args)
    {
        ReadEnvFileIfExists();
        var webApplicationOptions = new WebApplicationOptions
        {
            Args = args,
        };

        var builder = WebApplication.CreateBuilder(webApplicationOptions)
            .InitConfiguration(args)
            .InitLoggerConfiguration();

        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(builder.Configuration);
        if (smusdiOptions.NoVersioning != true)
        {
            builder.Services
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        builder.Services
            .AddSingleton<IFileSystem, FileSystem>()
            .AddAllHealthChecks()
            .AddEndpointsApiExplorer()
            .AddResponseCompression(smusdiOptions)
            .AddProblemDetails()
            .AddSwagger(builder.Configuration)
            .AddSecurity(builder.Configuration)
            .AddClientSecurity(builder.Configuration)
            .AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

        var mvcBuilder = builder.Services.AddControllers().AddJsonOptions(j => j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        mvcBuilder.AddParts(smusdiOptions);

        builder.Services
            .AddCors(o => o.AddPolicy(CorsPolicy, builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

        builder.Services
            .AddControllersInputValidation(smusdiOptions)
            .AddSingleton(TimeProvider.System)
            .AddScoped(typeof(IPipelineBuilder<>), typeof(PipelineBuilder<>))
            .SetupMultipartMaxSizes(smusdiOptions)
            .AddJsonSerializerWithJsonOptions()
            .AddApplicationServices<IServicesRegistrator>(builder.Configuration)
            .AddBeforeRunImplementations(builder.Configuration);

        this.WebApplicationBuilder = builder;
    }

    public virtual void Build()
    {
        if (this.WebApplicationBuilder == null)
        {
            return;
        }

        this.WebApplication = this.WebApplicationBuilder.Build();
    }

    public virtual void ConfigureWebApplication()
    {
        if (this.WebApplication == null)
        {
            return;
        }

        var smusdiOptions = SmusdiOptions.GetSmusdiOptions(this.WebApplication.Configuration);

        this.WebApplication
            .UseResponseCompression(smusdiOptions);

        if (smusdiOptions.StaticSites.Count > 0)
        {
            foreach (var site in smusdiOptions.StaticSites)
            {
                var folder = Path.Combine(this.WebApplication.Environment.ContentRootPath, site.Folder);
                if (Directory.Exists(folder))
                {
                    var options = new DefaultFilesOptions
                    {
                        RequestPath = new PathString(site.RequestPath),
                        FileProvider = new PhysicalFileProvider(folder),
                    };

                    this.WebApplication.UseDefaultFiles(options);
                }
            }

            foreach (var site in smusdiOptions.StaticSites)
            {
                var folder = Path.Combine(this.WebApplication.Environment.ContentRootPath, site.Folder);
                if (Directory.Exists(folder))
                {
                    this.WebApplication.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(folder),
                        RequestPath = site.RequestPath,
                        ServeUnknownFileTypes = site.ServeUnknownFileTypes,
                    });
                }
            }
        }

        this.WebApplication
            .UseCors(CorsPolicy)
            .UseHttpLogging()
            .UseExceptionHandler()
            .UseStatusCodePages();

        this.WebApplication.UseSecurity(this.WebApplication.Configuration);
        this.WebApplication.MapControllers();
        this.WebApplication
            .UseSwagger(this.WebApplication.Configuration)
            .UseHealthChecks()
            .UseInfoEndpoint()
            .ApplyCustomConfigurators();
    }

    public virtual Task RunAsync() => this.WebApplication?.RunAsync() ?? Task.CompletedTask;

    public virtual void Run() => this.WebApplication?.Run();

    public async Task ExecuteBeforeRunImplementations()
    {
        if (this.WebApplication != null)
        {
            using (var scope = this.WebApplication.Services.CreateScope())
            {
                var beforeRunImplementations = scope.ServiceProvider.GetServices<IBeforeRun>();
                foreach (var beforeRunImplementation in beforeRunImplementations)
                {
                    await beforeRunImplementation.Execute();
                }
            }
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && this.WebApplication != null)
        {
            this.WebApplication?.DisposeAsync().GetAwaiter().GetResult();
            this.WebApplication = null;
            this.WebApplicationBuilder = null;
        }
    }
}
