namespace Smusdi.Core.Pipeline;

public class PipelineStep<TContext>
{
    internal PipelineStep(string name, Func<PipelineContext<TContext>, Task> action)
    {
        this.Name = name;
        this.Action = action;
    }

    public string Name { get; }

    public Func<PipelineContext<TContext>, Task> Action { get; }
}
