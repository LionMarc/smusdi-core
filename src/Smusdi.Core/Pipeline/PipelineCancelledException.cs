using System.Runtime.Serialization;

namespace Smusdi.Core.Pipeline;

[Serializable]
public sealed class PipelineCancelledException : Exception
{
    public PipelineCancelledException()
        : base()
    {
    }

    private PipelineCancelledException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}
