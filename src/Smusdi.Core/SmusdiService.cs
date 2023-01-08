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
using Smusdi.Core.Swagger;
using Smusdi.Core.Validation;

namespace Smusdi.Core;

public class SmusdiService : IDisposable
{
    public const string CorsPolicy = "MyCorsPolicy";

    public WebApplicationBuilder? WebApplicationBuilder { get; private set; }

    public WebApplication? WebApplication { get; private set; }

    public static void InitAndRun(string[] args) => InitAndRun<SmusdiService>(args);

    public static void InitAndRun<T>(string[] args)
        where T : SmusdiService, new()
    {
        var service = new T();
        service.CreateAndInitializeBuider(args);
        service.Build();
        service.ConfigureWebApplication();

        service.Run();
    }

    public virtual void CreateAndInitializeBuider(string[] args)
    {
        var webApplicationOptions = new WebApplicationOptions
        {
            Args = args,
        };

        var builder = WebApplication.CreateBuilder(webApplicationOptions)
            .InitConfiguration()
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
            .AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

        var mvcBuilder = builder.Services.AddControllers().AddJsonOptions(j => j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        mvcBuilder.AddParts(smusdiOptions);

        builder.Services
            .AddCors(o => o.AddPolicy(CorsPolicy, builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

        builder.Services
            .AddControllersInputValidation(smusdiOptions)
            .AddSingleton<IClock, Clock>()
            .SetupMultipartMaxSizes(smusdiOptions)
            .AddJsonSerializerWithJsonOptions();

        builder.Services.AddApplicationServices<IServicesRegistrator>(builder.Configuration);

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
            .UseResponseCompression(smusdiOptions)
            .UseSwagger(this.WebApplication.Configuration)
            .UseHealthChecks()
            .UseInfoEndpoint()
            .ApplyCustomConfigurators();
    }

    public virtual Task RunAsync() => this.WebApplication?.RunAsync() ?? Task.CompletedTask;

    public virtual void Run() => this.WebApplication?.Run();

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
