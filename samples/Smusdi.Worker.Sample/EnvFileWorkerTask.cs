using Serilog;
using Smusdi.Core.Worker;

namespace Smusdi.Worker.Sample;

internal sealed class EnvFileWorkerTask(ILogger logger) : IWorkerTask
{
    public string Name => "For testing env file";

    public int Order => 1000;

    public Task Execute(CancellationToken stoppingToken)
    {
        const string environmentVariable = "TEST_ENV_FILE";
        var message = $"{environmentVariable} = {Environment.GetEnvironmentVariable(environmentVariable)}";
        logger.Information(message);
        return Task.CompletedTask;
    }
}
