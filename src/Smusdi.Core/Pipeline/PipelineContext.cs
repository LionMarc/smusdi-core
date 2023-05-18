namespace Smusdi.Core.Pipeline;

public sealed class PipelineContext<TContext>
{
    public PipelineContext(TContext payload) => this.Payload = payload;

    public TContext Payload { get; }

    public string CurrentStep { get; internal set; } = string.Empty;

    public PipelineState State { get; internal set; } = PipelineState.None;

    public Exception? CaughtException { get; internal set; }

    public Exception? FinallyException { get; internal set; }

    public bool IsCancelled { get; private set; }

    public void CancelPipeline() => this.IsCancelled = true;
}
