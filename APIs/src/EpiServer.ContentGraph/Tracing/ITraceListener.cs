namespace EPiServer.ContentGraph.Tracing
{
    public interface ITraceListener
    {
        void Add(ITraceEvent traceEvent);
    }
}
