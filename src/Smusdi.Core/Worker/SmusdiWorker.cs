﻿using System.IO.Abstractions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Smusdi.Core.Configuration;
using Smusdi.Core.Extensibility;
using Smusdi.Core.Helpers;
using Smusdi.Core.Json;
using Smusdi.Core.Logging;
using Smusdi.Core.Pipeline;
using Smusdi.Extensibility;

namespace Smusdi.Core.Worker;

public class SmusdiWorker
{
    public HostApplicationBuilder? HostBuilder { get; private set; }

    public IHost? WorkerHost { get; private set; }

    public static Task InitAndRunAsync(string[] args) => InitAndRunAsync<SmusdiWorker>(args);

    public static async Task InitAndRunAsync<T>(string[] args)
        where T : SmusdiWorker, new()
    {
        var worker = new T();
        worker.CreateAndInitializeBuider(args);
        worker.Build();
        worker.ConfigureHost();

        if (worker.WorkerHost != null)
        {
            await worker.WorkerHost.RunAsync();
        }
    }

    public virtual void CreateAndInitializeBuider(string[] args)
    {
        EnvFileHelper.ReadEnvFileIfExists();
        var builder = Host.CreateApplicationBuilder(args)
            .InitConfiguration(args)
            .InitLoggerConfiguration();

        builder.Services
            .AddSingleton<IFileSystem, FileSystem>()
            .AddSingleton(TimeProvider.System)
            .Configure<JsonOptions>(j => j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddJsonSerializerWithJsonOptions()
            .AddScoped(typeof(IPipelineBuilder<>), typeof(PipelineBuilder<>))
            .AddApplicationServices<IServicesRegistrator>(builder.Configuration)
            .AddWorkerTasks(builder.Configuration)
            .AddHostedService<WorkerTasksRunner>();

        this.HostBuilder = builder;
    }

    public virtual void Build()
    {
        if (this.HostBuilder == null)
        {
            return;
        }

        this.WorkerHost = this.HostBuilder.Build();
    }

    public virtual void ConfigureHost()
    {
        // Nothing for now
    }
}
