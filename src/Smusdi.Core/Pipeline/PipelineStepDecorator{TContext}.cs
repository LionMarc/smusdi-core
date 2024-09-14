using Serilog;
using Serilog.Context;

namespace Smusdi.Core.Pipeline;

public sealed class PipelineStepDecorator<TContext>(
    Func<PipelineContext<TContext>, Task> parentAction,
    Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task>? stepDecorator) : IPipelineStage<TContext>
{
    private readonly Func<PipelineContext<TContext>, Task> parentAction = parentAction;
    private readonly Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task>? stepDecorator = stepDecorator;

    public async Task Process(PipelineContext<TContext> context)
    {
        if (this.stepDecorator != null)
        {
            await this.stepDecorator(this.parentAction, context);
        }
        else
        {
            await this.parentAction(context);
        }
    }
}
