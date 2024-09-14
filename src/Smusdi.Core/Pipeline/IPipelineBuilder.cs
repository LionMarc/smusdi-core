namespace Smusdi.Core.Pipeline;

public interface IPipelineBuilder<TContext>
{
    IPipelineBuilder<TContext> Pipe(string name, Func<PipelineContext<TContext>, Task> action);

    /// <summary>
    /// Add a step from an implementation of <see cref="IPipelineStage{TContext}"/> registered as keyed scoped service.
    /// </summary>
    /// <param name="keyedService">The key used when registering the instance in the container.</param>
    /// <returns>The current instance.</returns>
    IPipelineBuilder<TContext> Pipe(string keyedService);

    IPipelineBuilder<TContext> Catch(Func<PipelineContext<TContext>, Task> action);

    IPipelineBuilder<TContext> Finally(Func<PipelineContext<TContext>, Task> action);

    IPipelineBuilder<TContext> AddStepDecorator(Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task> decorator);

    Pipeline<TContext> Build();
}
