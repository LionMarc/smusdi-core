using Serilog;

namespace Smusdi.Core.Pipeline;

internal sealed class PipelineBuilder<TContext> : IPipelineBuilder<TContext>
{
    private readonly ILogger logger;
    private readonly List<PipelineStep<TContext>> steps = new();
    private Func<PipelineContext<TContext>, Task>? catchAction;
    private Func<PipelineContext<TContext>, Task>? finallyAction;
    private Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task>? stepDecorator;

    public PipelineBuilder(ILogger logger) => this.logger = logger;

    public Pipeline<TContext> Build()
    {
        return new Pipeline<TContext>(this.logger, this.steps, this.catchAction, this.finallyAction, this.stepDecorator);
    }

    public IPipelineBuilder<TContext> Pipe(string name, Func<PipelineContext<TContext>, Task> action)
    {
        this.steps.Add(new PipelineStep<TContext>(name, action));
        return this;
    }

    public IPipelineBuilder<TContext> Catch(Func<PipelineContext<TContext>, Task> action)
    {
        this.catchAction = action;
        return this;
    }

    public IPipelineBuilder<TContext> Finally(Func<PipelineContext<TContext>, Task> action)
    {
        this.finallyAction = action;
        return this;
    }

    public IPipelineBuilder<TContext> AddStepDecorator(Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task> decorator)
    {
        this.stepDecorator = decorator;
        return this;
    }
}
