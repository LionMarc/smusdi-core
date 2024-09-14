namespace Smusdi.Core.Pipeline;

public interface IPipelineStage<TContext>
{
    Task Process(PipelineContext<TContext> context);
}
