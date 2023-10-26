namespace Smusdi.Core.Pipeline;

public interface IPipelineStepProcessor<TContext>
{
    string Name { get; }

    int Order { get; }

    Task Process(PipelineContext<TContext> context);
}
