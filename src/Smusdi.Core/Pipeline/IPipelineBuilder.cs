namespace Smusdi.Core.Pipeline;

public interface IPipelineBuilder<TContext>
{
    IPipelineBuilder<TContext> Pipe(string name, Func<PipelineContext<TContext>, Task> action);

    IPipelineBuilder<TContext> Catch(Func<PipelineContext<TContext>, Task> action);

    IPipelineBuilder<TContext> Finally(Func<PipelineContext<TContext>, Task> action);

    IPipelineBuilder<TContext> AddStepDecorator(Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task> decorator);

    Pipeline<TContext> Build();
}
