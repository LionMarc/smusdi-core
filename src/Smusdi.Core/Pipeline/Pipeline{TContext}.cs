using Serilog;
using Serilog.Context;

namespace Smusdi.Core.Pipeline;

public sealed class Pipeline<TContext>
{
    private readonly ILogger logger;
    private readonly IEnumerable<PipelineStep<TContext>> steps;
    private readonly Func<PipelineContext<TContext>, Task>? catchAction;
    private readonly Func<PipelineContext<TContext>, Task>? finallyAction;
    private readonly List<Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task>> stepDecorators;

    internal Pipeline(
        ILogger logger,
        IEnumerable<PipelineStep<TContext>> steps,
        Func<PipelineContext<TContext>, Task>? catchAction,
        Func<PipelineContext<TContext>, Task>? finallyAction,
        IEnumerable<Func<Func<PipelineContext<TContext>, Task>, PipelineContext<TContext>, Task>> stepDecorators)
    {
        this.logger = logger;
        this.steps = steps;
        this.catchAction = catchAction;
        this.finallyAction = finallyAction;
        this.stepDecorators = stepDecorators.ToList();
    }

    public async Task Run(PipelineContext<TContext> context)
    {
        context.State = PipelineState.Running;

        try
        {
            foreach (var step in this.steps)
            {
                using (LogContext.PushProperty(nameof(context.CurrentStep), context.CurrentStep))
                {
                    await this.ProcessStep(step, context);
                    if (context.IsCancelled)
                    {
                        this.logger.Information("Pipeline has been cancelled.");
                        break;
                    }
                }
            }

            context.State = context.IsCancelled ? PipelineState.Cancelled : PipelineState.Done;
        }
        catch (PipelineCancelledException e)
        {
            this.logger.Information(e, "Pipeline has been cancelled by step {CurrentStep}.", context.CurrentStep);
            context.State = PipelineState.Cancelled;
        }
        catch (Exception exception)
        {
            this.logger.Error(exception, "Unexpected exception: {ErrorMessage}", exception.Message);
            context.CaughtException = exception;
            if (this.catchAction != null)
            {
                context.State = PipelineState.Ko;
                try
                {
                    await this.catchAction(context);
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "Unexpected exception when calling the catch action: {ErrorMessage}", ex.Message);
                    context.State = PipelineState.Fatal;
                }
            }
            else
            {
                context.State = PipelineState.Fatal;
            }
        }
        finally
        {
            if (this.finallyAction != null)
            {
                this.logger.Information("Executing finally action.");
                try
                {
                    await this.finallyAction(context);
                }
                catch (Exception e)
                {
                    this.logger.Error(e, "Exception caught while executing finally action: {ErrorMessage}", e.Message);
                    context.FinallyException = e;
                    context.State = PipelineState.Fatal;
                }
            }
        }
    }

    private async Task ProcessStep(PipelineStep<TContext> step, PipelineContext<TContext> context)
    {
        using (LogContext.PushProperty("stepName", step.Name))
        {
            this.logger.Information("Executing step");
            context.CurrentStep = step.Name;
            if (this.stepDecorators.Count > 0)
            {
                var decoratedStep = new PipelineStepDecorator<TContext>(step.Action, null);
                for (var index = this.stepDecorators.Count - 1; index >= 0; index--)
                {
                    decoratedStep = new PipelineStepDecorator<TContext>(decoratedStep.Process, this.stepDecorators[index]);
                }

                await decoratedStep.Process(context);
            }
            else
            {
                await step.Action(context);
            }
        }
    }
}
