namespace EPiServer.ContentGraph.Tracing
{
    public interface ITraceEvent
    {
        DateTime Timestamp { get; }
        ITraceable Source { get; }
        string Message { get; }
        bool IsError { get; }
        Exception Exception { get; set; }
    }

    public class TraceEvent : ITraceEvent
    {

        public TraceEvent(ITraceable source, string message)
        { 
            Timestamp = DateTime.Now;
            Source = source;
            Message = message;
        }

        public DateTime Timestamp { get; private set; }
        public ITraceable Source { get; private set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public Exception Exception{get; set; }
    }
}
