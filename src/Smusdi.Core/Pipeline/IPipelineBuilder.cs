namespace Smusdi.Core.Pipeline;

public interface IPipelineBuilder<TContext>
{
    IPipelineBuilder<TContext> Pipe(string name, Func<PipelineContext<TContext>, Task> action);

    IPipelineBuilder<TContext> Catch(Func<PipelineContext<TContext>, Task> action);

    IPipelineBuilder<TContext> Finally(Func<PipelineContext<TContext>, Task> action);

    Pipeline<TContext> Build();
}
