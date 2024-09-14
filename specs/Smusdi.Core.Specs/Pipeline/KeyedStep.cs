using Smusdi.Core.Pipeline;

namespace Smusdi.Core.Specs.Pipeline;

public class KeyedStep(string name) : IPipelineStage<PipelineTestingContext>
{
    public string Name { get; } = name;

    public Task Process(PipelineContext<PipelineTestingContext> context)
    {
        context.Payload.CalledSteps.Add(this.Name);
        return Task.CompletedTask;
    }
}
