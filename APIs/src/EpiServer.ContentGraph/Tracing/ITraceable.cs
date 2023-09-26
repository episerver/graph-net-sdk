using System;

namespace EPiServer.ContentGraph.Tracing
{
    public interface ITraceable
    {
        Guid TraceId { get; }
    }
}
