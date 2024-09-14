namespace Smusdi.Core.Pipeline;

public interface IPipelineStepProcessor<TContext> : IPipelineStage<TContext>
{
    string Name { get; }

    int Order { get; }
}
